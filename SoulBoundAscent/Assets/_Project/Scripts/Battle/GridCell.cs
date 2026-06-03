using UnityEngine;

public class GridCell : MonoBehaviour
{
    public enum ZoneType { Player, Neutral, Enemy }

    public Vector2Int gridPos;
    public ZoneType zone;
    public bool isOccupied;

    private void OnMouseDown()
    {
        Debug.Log($"Cell {gridPos} clicked — Zone: {zone}, Occupied: {isOccupied}");
    }
}
