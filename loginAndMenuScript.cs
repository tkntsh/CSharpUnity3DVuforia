using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class loginAndMenuScript : MonoBehaviour
{
    //gameobjects needed
    public GameObject usernameFieldObject;
    public GameObject passwordFieldObject;
    public GameObject menuPanel;
    public GameObject loginPanel;
    public GameObject loginErrorText;
    public GameObject characterViewPanel;
    public GameObject scrollMenu;
    public GameObject menuBackground;
    public GameObject chooseCharacterText;
    public GameObject lightModeButton;
    public GameObject darkModeButton;

    //cameras to turn on and off
    public GameObject cameraOne;
    public GameObject cameraTwo;
    public GameObject cameraThree;

    //cameras to check if user touched the screen
    public Camera mainCam;
    public Camera archerCam;
    public Camera wizardCam;

    //characters and text for each character
    public GameObject ninja;
    public GameObject archer;
    public GameObject wizard;
    public GameObject messageText;
    public GameObject messageTextBackground;

    //changing theme of application
    public RawImage image1;
    public RawImage image2;
    public Texture textureLight1;
    public Texture textureDark1;
    public Texture textureLight2;
    public Texture textureDark2;

    public MeshRenderer mR1;
    public MeshRenderer mR2;
    public MeshRenderer mR3;

    //list for users in json file
    private List<User> userList;

    //string objects to store different facts on the ninja
    private string[] ninjaFacts = {"Ninja\nFrom: Japan\nSpecial skill: Hand-to-Hand combat\nWeakness: Sunlight",
                                   "Ninja\nFrom: Japan\nSpecial skill: Stealth\nWeakness: Loud noises", 
                                   "Ninja\nFrom: Japan\nSpecial skill: Throwing stars\nWeakness: Isolation",
                                   "Ninja\nFrom: Japan\nSpecial skill: Hand signals\nWeakness: Cake",
                                   "Ninja\nFrom: Japan\nSpecial skill: Camouflage\nWeakness: "};
    //string objects to store different facts on the archer
    private string[] archerFacts = {"Archer\nFrom: England\nSpecial skill: 3 bows with one shot\nWeakness: Hand-to-Hand combat",
                                    "Archer\nFrom: England\nSpecial skill: Exceptional eyesight\nWeakness: Close combat",
                                    "Archer\nFrom: England\nSpecial skill: Electric bow\nWeakness: Low defense",
                                    "Archer\nFrom: England\nSpecial skill: Blind shooting\nWeakness: Limited range",
                                    "Archer\nFrom: England\nSpecial skill: Silent shooter\nWeakness: Reload time"};
    //string objects to store different facts on the wizard
    private string[] wizardFacts = {"Wizard\nFrom: Italy\nSpecial skill: Manipulating reality\nWeakness: Endurance",
                                    "Wizard\nFrom: Italy\nSpecial skill: Mastery of elemental magic\nWeakness: Limited mobility",
                                    "Wizard\nFrom: Italy\nSpecial skill: Shapeshifting\nWeakness: Rain",
                                    "Wizard\nFrom: Italy\nSpecial skill: Element magic\nWeakness: Disruption",
                                    "Wizard\nFrom: Italy\nSpecial skill: Invisibility\nWeakness: Pyhsical defense"};
    //keeping track of which skills are being displayed for each character
    private int ninjaIndex = 0;
    private int archerIndex = 0;
    private int wizardIndex = 0;

    //beginning program with check if character objects are null
    void Start()
    {
        //pre-load users in json file to check against
        StartCoroutine(LoadUserData());

        //checking if gameobjects are null
        if (ninja == null || messageText == null || archer == null || wizard == null)
        {
            //error message
            Debug.LogError("Please assign all game objects and message texts.");
        }
    }
    //update method to check if touch has been used on screen
    void Update()
    {
        //checking if at least 1 finger has touched the screen
        if (Input.touchCount > 0)
        {
            //getting user touch per frame update
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                CheckTouch(touch.position, mainCam);
                CheckTouch(touch.position, archerCam);
                CheckTouch(touch.position, wizardCam);
            }
        }

        //mouse click tester
        if (Input.GetMouseButtonDown(0))
        {
            CheckTouch(Input.mousePosition, mainCam);
            CheckTouch(Input.mousePosition, archerCam);
            CheckTouch(Input.mousePosition, wizardCam);
        }
    }

    //method to highlight mesh
    public void meshRendererClickedNinja()
    {
        if(mR1 != null)
        {
            mR1.enabled = !mR1.enabled;
        }
        else
        {
            Debug.LogError("Error: Mesh renderer not found!");
        }
    }
    public void meshRendererClickedWizard()
    {
        if (mR2 != null)
        {
            mR2.enabled = !mR1.enabled;
        }
        else
        {
            Debug.LogError("Error: Mesh renderer not found!");
        }
    }
    public void meshRendererClickedArcher()
    {
        if (mR3 != null)
        {
            mR3.enabled = !mR1.enabled;
        }
        else
        {
            Debug.LogError("Error: Mesh renderer not found!");
        }
    }

    //method to change images to match dark colour scheme (main menu)
    public void setLightModeMainMenu()
    {
        if(image1 != null && textureLight1 != null)
        {
            image1.texture = textureLight1;
            image2.texture = textureLight2;
        }
    }
    //method to change images to match dark colour scheme (main menu)
    public void setDarkModeMainMenu()
    {
        if (image1 != null && textureDark1 != null)
        {
            image1.texture = textureDark1;
            image2.texture = textureDark2;
        }
    }
    //method to change images to match dark colour scheme (character menu)
    public void setLightModeSelectCharacterMenu()
    {
        if (image1 != null && textureLight2 != null)
        {
            image1.texture = textureLight1;
            image2.texture = textureLight2;
        }
    }
    //method to change images to match dark colour scheme (character menu)
    public void setDarkModeSelectCharacterMenu()
    {
        if (image1 != null && textureDark2 != null)
        {
            image1.texture = textureDark1;
            image2.texture = textureDark2;
        }
    }

    //method to check what object is being touched in what camera
    void CheckTouch(Vector2 screenPosition, Camera cam)
    {
        //creating a ray and raycast object to store information on what the ray intersects
        Ray ray = cam.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        //checks if any collider has been hit
        if (Physics.Raycast(ray, out hit))
        {
            //checking if ray hit ninja gameobject
            if(hit.transform == ninja.transform && cameraOne.gameObject.activeSelf)
            {
                messageText.GetComponent<Text>().text = ninjaFacts[ninjaIndex];
                messageTextBackground.SetActive(true);
                messageText.SetActive(true);
                //displaying different facts each time gameobject is clicked
                ninjaIndex = (ninjaIndex + 1) % ninjaFacts.Length;
            }
            //checking if ray hit archer gameobject
            else if(hit.transform == archer.transform && cameraTwo.gameObject.activeSelf)
            {;
                messageText.GetComponent<Text>().text = archerFacts[archerIndex];
                messageTextBackground.SetActive(true);
                messageText.SetActive(true);
                archerIndex = (archerIndex + 1) % archerFacts.Length;
            }
            //checking if ray hit wizard gameobject
            else if(hit.transform == wizard.transform && cameraThree.gameObject.activeSelf)
            {
                messageText.GetComponent<Text>().text = wizardFacts[wizardIndex];
                messageTextBackground.SetActive(true);
                messageText.SetActive(true);
                wizardIndex = (wizardIndex + 1) % wizardFacts.Length;
            }
        }
        else
        {
            //error for if raycast doesn't work or can't find character
            Debug.LogError("Error: Couldn't select character");
        }
    }

    //method to load the users from the json file
    IEnumerator LoadUserData()
    {
        //storing website where needed date is
        UnityWebRequest request = UnityWebRequest.Get("https://jsonplaceholder.typicode.com/users");
        yield return request.SendWebRequest();
        //checking if the data was successfully fetched from the website
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error fetching user data: " + request.error);
        }
        //if the connection is successful it downloads and stores the text in the userList object to read from later
        else
        {
            string json = request.downloadHandler.text;
            userList = JsonUtility.FromJson<UserList>("{\"users\":" + json + "}").users;
        }
    }
    //method to check details used to log in by user
    public void CheckLogin()
    {
        string enteredUsername = usernameFieldObject.GetComponent<Text>().text;
        string enteredPassword = passwordFieldObject.GetComponent<Text>().text;
        //checking each user in the api list to see if any match
        foreach (User user in userList)
        {
            //checking username and password
            if (user.username == enteredUsername && user.address.zipcode == enteredPassword)
            {
                PlayerPrefs.SetString("Username", enteredUsername);
                PlayerPrefs.SetString("Zipcode", enteredPassword);
                //saving user details
                PlayerPrefs.Save();

                //switching panels and turning error message off
                loginPanel.SetActive(false);
                menuPanel.SetActive(true);
                loginErrorText.SetActive(false);
                return;
            }
            else
            {
                //displaying error to user
                loginErrorText.SetActive(true);
            }
        }
        //checking error
        Debug.Log("Invalid username or password");
    }

    //taking user back to main menu
    public void exit3DCharacterView()
    {
        characterViewPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    //view ninja character
    public void viewNinjaButton()
    {
        //setting cameras off
        cameraThree.SetActive(false);
        cameraTwo.SetActive(false);
        cameraOne.SetActive(true);
        //setting text and text panel off
        messageText.SetActive(false);
        messageTextBackground.SetActive(false);
        //turning scroll menu, text and buttons off
        scrollMenu.SetActive(false);
        menuBackground.SetActive(false);
        chooseCharacterText.SetActive(false);
        lightModeButton.SetActive(false);
        darkModeButton.SetActive(false);
        //setting panel to view character on
        characterViewPanel.SetActive(true);
    }
    //view archer character
    public void viewArcherButton()
    {
        //setting cameras off
        cameraThree.SetActive(false);
        cameraOne.SetActive(false);
        cameraTwo.SetActive(true);
        //setting text and text panel off
        messageText.SetActive(false);
        messageTextBackground.SetActive(false);
        //turning scroll menu, text and buttons off
        scrollMenu.SetActive(false);
        menuBackground.SetActive(false);
        chooseCharacterText.SetActive(false);
        lightModeButton.SetActive(false);
        darkModeButton.SetActive(false);
        //setting panel to view character on
        characterViewPanel.SetActive(true);
    }
    //view wizard character
    public void viewWizardButton()
    {
        //setting cameras off
        cameraTwo.SetActive(false);
        cameraOne.SetActive(false);
        cameraThree.SetActive(true);
        //setting text and text panel off
        messageText.SetActive(false);
        messageTextBackground.SetActive(false);
        //turning scroll menu, text and buttons off
        scrollMenu.SetActive(false);
        menuBackground.SetActive(false);
        chooseCharacterText.SetActive(false);
        lightModeButton.SetActive(false);
        darkModeButton.SetActive(false);
        //setting panel to view character on
        characterViewPanel.SetActive(true);
    }

    //switch to character view in arcade scene
    public void switchTo3DCharacter()
    {
        menuPanel.SetActive(false);
        scrollMenu.SetActive(true);
    }

    //switching to AR scene
    public void switchToARScene()
    {
        SceneManager.LoadScene("arScene");
        menuPanel.SetActive(true);
    }
}
//classes to break up different parts of users in json file

//address section of json info
[System.Serializable]
public class Address
{
    public string street;
    public string suite;
    public string city;
    public string zipcode;
    public Geo geo;
}
//geo section of json info
[System.Serializable]
public class Geo
{
    public string lat;
    public string lng;
}
//user details section of json info
[System.Serializable]
public class User
{
    public int id;
    public string name;
    public string username;
    public string email;
    public Address address;
    public string phone;
    public string website;
    public Company company;
}
//company section of json info
[System.Serializable]
public class Company
{
    public string name;
    public string catchPhrase;
    public string bs;
}
//creating a list to store all the users from the json file
[System.Serializable]
public class UserList
{
    public List<User> users;
}