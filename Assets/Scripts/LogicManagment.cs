using System.Linq;
using TMPro;
using UnityEngine;

public class LogicManagment : MonoBehaviour
{
    private bool isBlueTurn;

    [SerializeField] TextMeshProUGUI OrangeBanner;
    [SerializeField] TextMeshProUGUI BlueBanner;
    [SerializeField] float rotationSpeed = 1;
    private float startingOrientation;

    private int orangeStash = 12;
    private int blueStash = 12;

    // Start is called before the first frame update
    void Start()
    {
        isBlueTurn = true;

        BlueBanner.text = "Blue: " + blueStash;
        OrangeBanner.text = "Orange: " + orangeStash;

        startingOrientation = transform.rotation.x;
    }

    // Update is called once per frame
    void Update()
    {
        float horzInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");
        if(vertInput == 0)
        {
            //transform.rotation = Quaternion.Euler(0, transform.rotation.y, transform.rotation.z);
            while (transform.rotation.x < startingOrientation)
            {
                Debug.Log("Tilt");
                transform.Rotate(Vector3.forward * Time.deltaTime);
            }
        }

        transform.Rotate(Vector3.up, horzInput * rotationSpeed * Time.deltaTime);
        transform.Rotate(Vector3.right, vertInput * rotationSpeed * Time.deltaTime);
    }

    public bool GetTurn()
    {
        return isBlueTurn;
    }

    public void UpdateTurn()
    {
        isBlueTurn = !GetTurn();
    }

    public void Decrement(bool blue)
    {
        if (blue)
        {
            BlueBanner.text = "Blue: " + --blueStash;
        }
        else if (!blue)
        {            
            OrangeBanner.text = "Orange: " + --orangeStash;
        }
    }

    public void Increment(bool blue)
    {
        if (blue)
        {
            BlueBanner.text = "Blue: " + ++blueStash;
        }
        else if (!blue)
        {
            OrangeBanner.text = "Orange: " + ++orangeStash;
        }
    }
}
