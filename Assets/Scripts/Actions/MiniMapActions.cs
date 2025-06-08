using UnityEngine;

public class MiniMapActions : MonoBehaviour
{
    [SerializeField] private RectTransform _miniMapRect;
    [SerializeField] private GameObject _islandMarkerPrefab;
    [SerializeField] private GameObject _islandMarkerContainer;
    [SerializeField] private RectTransform _shipIcon;
    [SerializeField] private GameObject _shipModel;

    private WorldMapSettings _worldMapSettings;
    private IslandManager _islandManager;

    private void Start()
    {
        _islandManager = FindFirstObjectByType<IslandManager>();
        _worldMapSettings = FindFirstObjectByType<WorldMapSettings>();
        
        InstantiateIslandMarkers();
    }
    
    private void Update()
    {
        UpdateShipPosition();
        UpdateShipRotation();
    }

    private void InstantiateIslandMarkers()
    {
        foreach (var island in _islandManager.IslandList)
        {
            var marker = Instantiate(_islandMarkerPrefab, _islandMarkerContainer.transform).GetComponent<RectTransform>();;
            
            marker.anchoredPosition = ConvertWorldPositionToMinimapCoordinates(island.IslandObject);
        }
    }

    private Vector2 ConvertWorldPositionToMinimapCoordinates(GameObject worldObject)
    {
        var normalizedPosition = new Vector2(
            (worldObject.transform.position.x - _worldMapSettings.WorldMapCenter.x) / _worldMapSettings.WorldMapSize.x + 0.5f,
            (worldObject.transform.position.z - _worldMapSettings.WorldMapCenter.z) / _worldMapSettings.WorldMapSize.y + 0.5f
        );
        
        var minimapPosition = new Vector2(
            (normalizedPosition.x - 0.5f) * _miniMapRect.rect.width,
            (normalizedPosition.y - 0.5f) * _miniMapRect.rect.height
        );

        return minimapPosition;
    }

    private void UpdateShipPosition()
    {
        _shipIcon.anchoredPosition = ConvertWorldPositionToMinimapCoordinates(_shipModel);
    }

    private void UpdateShipRotation()
    {
        var shipModelRotationY = _shipModel.transform.eulerAngles.y;
        var scaleXAxis = shipModelRotationY > 180f ? -Mathf.Abs(_shipIcon.localScale.x) : Mathf.Abs(_shipIcon.localScale.x);
        _shipIcon.localScale = new Vector3(scaleXAxis, _shipIcon.localScale.y, _shipIcon.localScale.z);
    }
}
