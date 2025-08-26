using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


public class ARPlaceObjectUI : MonoBehaviour
{
    [SerializeField] private ARRaycastManager raycastManager;

    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private GameObject spherePrefab;
    [SerializeField] private GameObject cylinderPrefab;

    private GameObject selectedPrefab;
    private bool isPlacing = false;

    // Keep track of buttons to highlight active one
    private Button cubeButton;
    private Button sphereButton;
    private Button cylinderButton;
    private Button goBackButton;

    void Start()
    {



        selectedPrefab = cubePrefab; // default

        // Get UI
        var uiDocument = FindObjectOfType<UIDocument>();
        var root = uiDocument.rootVisualElement;
        if (uiDocument != null)
        {


            cubeButton = root.Q<Button>("CubeButton");
            sphereButton = root.Q<Button>("SphereButton");
            cylinderButton = root.Q<Button>("CylinderButton");

            if (cubeButton != null) cubeButton.clicked += () => SelectPrefab(cubePrefab, cubeButton);
            if (sphereButton != null) sphereButton.clicked += () => SelectPrefab(spherePrefab, sphereButton);
            if (cylinderButton != null) cylinderButton.clicked += () => SelectPrefab(cylinderPrefab, cylinderButton);

            HighlightActive(cubeButton); // highlight default


            Debug.Log($"CubeButton found? {cubeButton != null}");
            Debug.Log($"SphereButton found? {sphereButton != null}");
            Debug.Log($"CylinderButton found? {cylinderButton != null}");
        }

        Button goBackButton = root.Q<Button>("BackButton");
        if (goBackButton != null)
            goBackButton.clicked += () =>
            {
                Debug.Log("Loading GameScene...");
                SceneManager.LoadScene("StartScreen");
            };

    }

    void Update()
    {
        if (!raycastManager) return;

        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began ||
             Input.GetMouseButtonDown(0)) && !isPlacing)
        {
            isPlacing = true;

            Vector2 touchPosition = (Input.touchCount > 0) ?
                Input.GetTouch(0).position :
                (Vector2)Input.mousePosition;

            PlaceObject(touchPosition);
        }
    }

    void PlaceObject(Vector2 touchPosition)
    {
        var rayHits = new List<ARRaycastHit>();
        raycastManager.Raycast(touchPosition, rayHits, TrackableType.PlaneWithinPolygon);

        if (rayHits.Count > 0 && selectedPrefab != null)
        {
            Pose hitPose = rayHits[0].pose;
            Instantiate(selectedPrefab, hitPose.position, hitPose.rotation);
        }

        StartCoroutine(SetIsPlacingToFalseWithDelay());
    }

    IEnumerator SetIsPlacingToFalseWithDelay()
    {
        yield return new WaitForSeconds(0.25f);
        isPlacing = false;
    }

    void SelectPrefab(GameObject prefab, Button activeButton)
    {
        selectedPrefab = prefab;
        Debug.Log($"Selected prefab: {prefab.name}");
        HighlightActive(activeButton);
    }

    void HighlightActive(Button active)
    {
        // Reset all
        if (cubeButton != null) cubeButton.style.backgroundColor = new StyleColor(Color.white);
        if (sphereButton != null) sphereButton.style.backgroundColor = new StyleColor(Color.white);
        if (cylinderButton != null) cylinderButton.style.backgroundColor = new StyleColor(Color.white);

        // Highlight chosen one
        if (active != null) active.style.backgroundColor = new StyleColor(Color.green);
    }
}
