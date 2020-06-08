using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopulationManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _botPrefab;

    [SerializeField]
    private int _populationSize = 50;

    [SerializeField]
    private float trailTime = 5;

    private List<GameObject> _population = new List<GameObject>();

    private static float _elapsed = 0;
    
    private int _generation = 1;

    private GUIStyle _guiStyle = new GUIStyle();

    private void OnGUI()
    {
        _guiStyle.fontSize = 25;
        _guiStyle.normal.textColor = Color.white;

        GUI.BeginGroup(new Rect(10, 10, 250, 250));
        GUI.Box(new Rect(0, 0, 140, 140), "Stats", _guiStyle);
        GUI.Label(new Rect(10, 25, 200, 30), "Gen: " + _generation, _guiStyle);
        GUI.Label(new Rect(10, 50, 200, 30), string.Format("Time: {0:0.00}", _elapsed), _guiStyle);
        GUI.Label(new Rect(10, 75, 200, 30), "Population: " + _population.Count, _guiStyle);
        GUI.EndGroup();
    }

    // Start is called before the first frame update
    private void Start()
    {
        for (int i = 0; i < _populationSize; i++)
        {
            Vector3 startingPosition = new Vector3(
                this.transform.position.x + Random.Range(-2, 2),
                this.transform.position.y,
                this.transform.position.z + Random.Range(-2, 2));

            GameObject bot = Instantiate(_botPrefab, startingPosition, this.transform.rotation);
            bot.GetComponent<Brain>().Init();
            _population.Add(bot);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        _elapsed += Time.deltaTime;
        if (_elapsed >= trailTime)
        {
            BreedNewPopulation();
            _elapsed = 0;
        }
    }

    private void BreedNewPopulation()
    {
        List<GameObject> sortedList = _population.OrderBy(bot => bot.GetComponent<Brain>().DistanceTravelled).ToList();

        _population.Clear();

        for (int i = (int)(sortedList.Count / 2.0f) - 1; i < sortedList.Count - 1; i++)
        {
            _population.Add(Breed(sortedList[i], sortedList[i + 1]));
            _population.Add(Breed(sortedList[i + 1], sortedList[i]));
        }

        // Destroy all parents and previous population
        for (int i = 0; i < sortedList.Count; i++)
        {
            Destroy(sortedList[i]);
        }

        _generation++;
    }

    private GameObject Breed(GameObject parent1, GameObject parent2)
    {
        Vector3 startingPosition = new Vector3(
            this.transform.position.x + Random.Range(-2, 2),
            this.transform.position.y,
            this.transform.position.z + Random.Range(-2, 2));

        GameObject offspring = Instantiate(_botPrefab, startingPosition, this.transform.rotation);

        Brain offspringBrain = offspring.GetComponent<Brain>();

        offspringBrain.Init();

        if (Random.Range(0, 100) == 1) // mutate 1 in 100
        {
            offspringBrain.DNA.Mutate();
        }
        else
        {
            offspringBrain.DNA.Combine(parent1.GetComponent<Brain>().DNA, parent2.GetComponent<Brain>().DNA);
        }

        return offspring;
    }
}