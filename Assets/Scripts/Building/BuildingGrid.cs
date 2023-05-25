using UnityEngine;

public class BuildingGrid : MonoBehaviour
{
    [SerializeField] private InputHandler _inputHandler;

    public Vector2Int GridSize = new Vector2Int(10, 10);

    public Building[,] grid;
    public Building flyingBuilding;
    private Camera mainCamera;
    private bool _isAvailable = true;
    private int _currentPositionX;
    private int _currentPositionY;
    private Plane plane;

    private void Awake()
    {
        grid = new Building[GridSize.x, GridSize.y];

        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        _inputHandler.LeftMouseButtonClecked += OnLeftMouseButtonClicked;
    }

    private void OnDisable()
    {
        _inputHandler.LeftMouseButtonClecked -= OnLeftMouseButtonClicked;
    }

    private void Update()
    {
        if (flyingBuilding != null)
        {
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            plane = groundPlane;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float position))
            {
                Vector3 worldPosition = ray.GetPoint(position);

                _currentPositionX = Mathf.RoundToInt(worldPosition.x);
                _currentPositionY = Mathf.RoundToInt(worldPosition.z);

                _isAvailable = true;

                if (_isAvailable && IsPlaceTaken(_currentPositionX, _currentPositionY))
                    _isAvailable = false;

                if (_currentPositionX < 0 || _currentPositionX > GridSize.x - flyingBuilding.Size.x)
                    _isAvailable = false;
                if (_currentPositionY < 0 || _currentPositionY > GridSize.y - flyingBuilding.Size.y)
                    _isAvailable = false;

                flyingBuilding.transform.position = new Vector3(_currentPositionX, 0, _currentPositionY);
                flyingBuilding.SetTranssparent(_isAvailable);
            }
        }
    }

    public void StartPlaicingBuilding(Building buildingPrefab)
    {
        if (flyingBuilding != null)
        {
            Destroy(flyingBuilding.gameObject);
        }

        flyingBuilding = Instantiate(buildingPrefab);
    }

    private bool IsPlaceTaken(int placeX, int placeY)
    {
        for (int x = 0; x < flyingBuilding.Size.x; x++)
        {
            for (int y = 0; y < flyingBuilding.Size.y; y++)
            {
                if (grid[placeX + x, placeY + y] != null)
                    return true;
            }
        }

        return false;
    }

    private void OnLeftMouseButtonClicked()
    {
        if (_isAvailable && flyingBuilding != null)
        {
            PlaceFlyingBuilding(_currentPositionX, _currentPositionY);
        }
    }

    private void PlaceFlyingBuilding(int placeX, int placeY)
    {
        for (int x = 0; x < flyingBuilding.Size.x; x++)
        {
            for (int y = 0; y < flyingBuilding.Size.y; y++)
            {
                grid[placeX + x, placeY + y] = flyingBuilding;
            }
        }

        flyingBuilding.SetNormal();
        flyingBuilding = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 1, 0.3f);
        Gizmos.DrawCube(plane.normal, new Vector3(GridSize.x, .1f, GridSize.y));
    }
}
