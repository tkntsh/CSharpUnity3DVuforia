using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class trackedImage : MonoBehaviour
{
    //creating text to display on canvas
    public GameObject ninjaText;
    private ARTrackedObjectManager trackedObjectManager;
    //checking if image is found or not with true or false
    private bool objectTracked = false;

    //method to run when application executes
    void Awake()
    {
        trackedObjectManager = FindObjectOfType<ARTrackedObjectManager>();
        ninjaText.SetActive(false);
    }

    //when object is detected
    void OnEnable()
    {
        trackedObjectManager.trackedObjectsChanged += OnTrackedObjectsChanged;
    }
    //when object is no longer detected
    void OnDisable()
    {
        trackedObjectManager.trackedObjectsChanged -= OnTrackedObjectsChanged;
    }

    //method to 
    void OnTrackedObjectsChanged(ARTrackedObjectsChangedEventArgs eventArgs)
    {
        if (!objectTracked)
        {
            foreach (var trackedObject in eventArgs.added)
            {
                UpdateDescription();
                //object detected
                objectTracked = true;
                break;
            }
        }
        else
        {
            foreach (var trackedObject in eventArgs.removed)
            {
                //when object isnt detected anymore switch off the text
                ninjaText.SetActive(false);
                objectTracked = false;
                break;
            }
        }
    }

    //method to print text on canvas
    void UpdateDescription()
    {
        //turning on text when object is detected
        ninjaText.SetActive(true);
        //text to display on canvas when image has been detected
        ninjaText.GetComponent<Text>().text = "NINJA\nFROM: JAPAN\nSPECIAL SKILLS: STEALTH\nWEAKNESSES: SUNLIGHT";
    }

    //button click back to UI scene
    public void switchToUIScene()
    {
        SceneManager.LoadScene("uiScene");
    }
}
