using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopulationManager : MonoBehaviour
{
    public GameObject _personPrefab;

    public int PopulatonSize = 10;

    public static float Elapsed = 0;

    private List<GameObject> _population = new List<GameObject>();

    private int _trialTime = 10;
    private int _generation = 0;

    // Start is called before the first frame update
    private void Start()
    {
        for (int i = 0; i < PopulatonSize; i++)
        {
            Vector3 position = new Vector3(Random.Range(-9, 9), Random.Range(-4.5f, 4.5f), 0);

            GameObject firstPeople = Instantiate(_personPrefab, position, Quaternion.identity);

            firstPeople.GetComponent<DNA>().R = Random.Range(0.0f, 1.0f);
            firstPeople.GetComponent<DNA>().G = Random.Range(0.0f, 1.0f);
            firstPeople.GetComponent<DNA>().B = Random.Range(0.0f, 1.0f);

            firstPeople.GetComponent<DNA>().Scale = Random.Range(0.1f, 0.3f);

            _population.Add(firstPeople);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        Elapsed += Time.deltaTime;

        if (Elapsed > _trialTime)
        {
            BreedNewPopulation();
            Elapsed = 0;
        }
    }

    private void OnGUI()
    {
        GUIStyle guiStyle = new GUIStyle();

        guiStyle.fontSize = 50;
        guiStyle.normal.textColor = Color.white;

        GUI.Label(new Rect(10, 10, 100, 20), "Generation: " + _generation, guiStyle);
        GUI.Label(new Rect(10, 65, 100, 20), "Trial Time: " + (int)Elapsed, guiStyle);
    }

    private void BreedNewPopulation()
    {
        List<GameObject> newPopulation = new List<GameObject>();
        List<GameObject> sortedPopulation = _population.OrderBy(person => person.GetComponent<DNA>().TimeToDie).ToList();

        _population.Clear();

        // Breed upper half of sorted list
        for (int i = (int)(sortedPopulation.Count / 2.0f) - 1; i < sortedPopulation.Count - 1; i++)
        {
            _population.Add(Breed(sortedPopulation[i], sortedPopulation[i + 1]));
            _population.Add(Breed(sortedPopulation[i + 1], sortedPopulation[i]));
        }

        // Destroy all parents and previous population
        for (int i = 0; i < sortedPopulation.Count; i++)
        {
            Destroy(sortedPopulation[i]);
        }

        _generation++;
    }

    private GameObject Breed(GameObject parent1, GameObject parent2)
    {
        Vector3 position = new Vector3(Random.Range(-9, 9), Random.Range(-4.5f, 4.5f), 0);
        GameObject offspring = Instantiate(_personPrefab, position, Quaternion.identity);

        DNA dnaParent1 = parent1.GetComponent<DNA>();
        DNA dnaParent2 = parent2.GetComponent<DNA>();

        // Swap parent DNA
        if (Random.Range(0, 1000) > 5)
        {
            offspring.GetComponent<DNA>().R = Random.Range(0, 10) < 5 ? dnaParent1.R : dnaParent2.R;
            offspring.GetComponent<DNA>().G = Random.Range(0, 10) < 5 ? dnaParent1.G : dnaParent2.G;
            offspring.GetComponent<DNA>().B = Random.Range(0, 10) < 5 ? dnaParent1.B : dnaParent2.B;

            offspring.GetComponent<DNA>().Scale = Random.Range(0, 10) < 5 ? dnaParent1.Scale : dnaParent2.Scale;
        }
        else // Mutation
        {
            offspring.GetComponent<DNA>().R = Random.Range(0.0f, 1.0f);
            offspring.GetComponent<DNA>().G = Random.Range(0.0f, 1.0f);
            offspring.GetComponent<DNA>().B = Random.Range(0.0f, 1.0f);

            offspring.GetComponent<DNA>().Scale = Random.Range(0.1f, 0.3f);
        }

        return offspring;
    }
}
