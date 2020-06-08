using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(ThirdPersonCharacter))]
public class Brain : MonoBehaviour
{
    [SerializeField]
    private float _timeAlive;
    public float TimeAlive 
    { 
        get 
        { 
            return _timeAlive; 
        } 
        set 
        { _timeAlive = value; 
        } 
    }

    [SerializeField]
    private float _distanceTravelled;
    public float DistanceTravelled 
    { 
        get 
        { 
            return _distanceTravelled; 
        } 
        set 
        { 
            _distanceTravelled = value; 
        } 
    }

    private DNA _dna;
    public DNA DNA 
    { 
        get 
        { 
            return _dna; 
        } 
        set 
        { 
            _dna = value; 
        } 
    }

    private readonly int _dnaLenght = 1;
    private ThirdPersonCharacter _character;
    private Vector3 _startPosition;
    private Vector3 _move;
    private bool _jump;
    private bool _crouch;
    private bool _alive;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("dead"))
        {
            _alive = false;
        }
    }

    public void Init()
    {
        // Initialize DNA
        // 0 forward
        // 1 back
        // 2 left
        // 3 right
        // 4 jump
        // 5 crouch

        _dna = new DNA(_dnaLenght, 6);
        _character = GetComponent<ThirdPersonCharacter>();
        _timeAlive = 0;
        _alive = true;
        _jump = false;
        _startPosition = this.transform.position;
    }

    private void FixedUpdate()
    {
        // Read DNA
        float horizontal = 0;
        float vertical = 0;
        _crouch = false;

        switch (_dna.GetGene(0))
        {
            case 0:
                vertical = 1;
                break;

            case 1:
                vertical = -1;
                break;

            case 2:
                horizontal = -1;
                break;

            case 3:
                horizontal = 1;
                break;

            case 4:
                _jump = true;
                break;

            case 5:
                _crouch = true;
                break;
        }

        _move = (vertical * Vector3.forward) + (horizontal * Vector3.right);
        _character.Move(_move, _crouch, _jump);
        _jump = false;


        if (_alive)
        {
            _timeAlive += Time.fixedDeltaTime;
            _distanceTravelled = Vector3.Distance(this.transform.position, _startPosition);
        }
    }
}