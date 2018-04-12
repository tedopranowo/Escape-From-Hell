using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minimap
{

    public class Vec2Int
    {
        public int x, y;
        public static Vec2Int zero { get { return new Vec2Int(0, 0); } }
        public static Vec2Int one { get { return new Vec2Int(1, 1); } }

        public Vec2Int(int _x, int _y)
        {
            x = _x;
            y = _y;
        }

        public bool Equals(Vec2Int other)
        {
            return (other.x == this.x && other.y == this.y);
        }
        public static bool operator ==(Vec2Int left, Vec2Int right)
        {
            return (left.x == right.x && left.y == right.y);
        }
        public static bool operator !=(Vec2Int left, Vec2Int right)
        {
            return (left.x != right.x || left.y != right.y);
        }
        public static Vec2Int operator +(Vec2Int left, Vec2Int right)
        {
            return new Vec2Int(left.x + right.x, left.y + right.y);
        }
        public static Vec2Int operator -(Vec2Int left, Vec2Int right)
        {
            return new Vec2Int(left.x - right.x, left.y - right.y);
        }
        public static Vec2Int operator *(Vec2Int left, Vec2Int right)
        {
            return new Vec2Int(left.x * right.x, left.y * right.y);
        }
        public static Vec2Int operator *(Vec2Int left, int right)
        {
            return new Vec2Int(left.x * right, left.y * right);
        }
        public Vec2Int Add(Vec2Int other)
        {
            return new Vec2Int( other.x + this.x , other.y + this.y);
        }
        public Vec2Int Subtract(Vec2Int other)
        {
            return new Vec2Int(other.x - this.x, other.y - this.y);
        }
    }

    public class MinimapManager : MonoBehaviour {

        Node _startNode = null;
        Node _currentNode = null;

        List<Node> _knownNodes = new List<Node>();

        Vector2 startPos = Vector2.zero;
        Vec2Int currentGrid;

        [SerializeField]
        GameObject _roomSprite;

        [SerializeField]
        GameObject _doorSprite;

        Canvas _canvas;
        [SerializeField]
        float _tileSize = 100.0f;
        [SerializeField]
        List<GameObject> _objectToEnable = new List<GameObject>();
        [SerializeField]
        GameObject _playerSprite = null;

        [SerializeField]
        GameObject _fakeSprite = null;

        bool _isEnabled = false;

	    // Use this for initialization
	    void Start ()
        {
            currentGrid = Vec2Int.zero;
            _roomSprite.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _tileSize);
            _roomSprite.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _tileSize);
            _canvas = GetComponent<Canvas>();
            startPos = FindObjectOfType<CameraManager>().targetRoom.transform.position;
            Vector2 currentRoomPos = FindObjectOfType<CameraManager>().targetRoom.transform.position;
            Vec2Int gridPosition = currentGrid;
            _startNode = new Node(currentRoomPos, gridPosition, _roomSprite, _canvas.transform, _fakeSprite);
            GetFakeRooms(currentRoomPos, _startNode);

            _currentNode = _startNode;
            _knownNodes.Add(_currentNode);
            _playerSprite.transform.SetAsLastSibling();
        }

        void GetFakeRooms(Vector2 currentRoomPos, Node addToNode)
        {
            //get all doors of the current room and add them as possible rooms.
            Door[] doors = FindObjectOfType<CameraManager>().targetRoom.transform.GetComponentsInChildren<Door>();
            for (int i = 0; i < doors.Length; ++i)
            {
                Vector2 position = doors[i].nextRoom.transform.position;
                Vector2 deltaPosition = position - currentRoomPos;
                if (deltaPosition.x != 0)
                    deltaPosition.x /= Mathf.Abs(deltaPosition.x);
                if (deltaPosition.y != 0)
                    deltaPosition.y /= Mathf.Abs(deltaPosition.y);
                bool hasRoom = false;
                for (int j = 0; j < _knownNodes.Count; ++j)
                {
                    _knownNodes[j].CheckFake(new Vec2Int((int)(currentGrid.x + deltaPosition.x), (int)(currentGrid.y + deltaPosition.y)));
                    //_knownNodes[j].CheckFake(new Vec2Int((int)(currentGrid.x), (int)(currentGrid.y)));
                    if (_knownNodes[j].gridPosition == new Vec2Int((int)(currentGrid.x + deltaPosition.x), (int)(currentGrid.y + deltaPosition.y)))
                    {
                        hasRoom = true;
                    }
                    //if (_knownNodes[j].gridPosition == new Vec2Int((int)(currentGrid.x), (int)(currentGrid.y)))
                    //{
                    //    hasRoom = true;
                    //}
                }
                if (!hasRoom)
                    addToNode.AddPossibleNeighboor(new Vec2Int((int)deltaPosition.x, (int)deltaPosition.y), _doorSprite);
            }
        }

        // Update is called once per frame
        void Update () {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                DrawGrid();
                _isEnabled = true;
            }
            if (Input.GetKeyUp(KeyCode.Tab))
            {

                _isEnabled = false;
                HideGrid();
            }

        }

        public void GoToRoom(Transform transform)
        {
            Vector2 delta = new Vector2(transform.position.x, transform.position.y) - _currentNode.worldPosition;
            if(delta.x != 0)
                delta.x /= Mathf.Abs(delta.x);
            if (delta.y != 0)
            {
                delta.y /= Mathf.Abs(delta.y);
            }
            Vec2Int myDelta = new Vec2Int((int)delta.x, (int)delta.y);
            currentGrid += myDelta;
            Node neighboor = _currentNode.HasNeighboor(currentGrid);
            if (neighboor == null)
            {
                for (int i = 0; i < _knownNodes.Count; ++i)
                {
                    if (_knownNodes[i].gridPosition == currentGrid)
                    {
                        neighboor = _knownNodes[i];
                        break;
                    }

                }
            }
            if (neighboor != null)
            {
                //currentGrid -= myDelta;
            }
            else
            {
                neighboor = new Node(transform.position, currentGrid, _roomSprite, _canvas.transform, _fakeSprite);
                _knownNodes.Add(neighboor);
                //loop though all the known nodes and see if any of them have a fake room that is in position of the room we just created
                for (int i = 0; i < _knownNodes.Count; ++i)
                {
                    _knownNodes[i].CheckFake(currentGrid);
                }
                GetFakeRooms(transform.position, neighboor);
                _playerSprite.transform.SetAsLastSibling();
                neighboor.AddNeighboor(_currentNode);
                _currentNode.AddNeighboor(neighboor);
            }
            _currentNode = neighboor;
            if (_isEnabled)
                DrawGrid();
        }

        [ContextMenu("Draw Grid")]
        public void DrawGrid()
        {
            HideGrid();
            for (int i = 0; i < _objectToEnable.Count; ++i)
            {
                _objectToEnable[i].SetActive(true);
            }
            Vec2Int roomOffset = _currentNode.gridPosition * -1;
            _currentNode.Draw(_tileSize, _canvas, roomOffset);
        }
        [ContextMenu("Hide Grid")]
        public void HideGrid()
        {
            for (int i = 0; i < _objectToEnable.Count; ++i)
            {
                _objectToEnable[i].SetActive(false);
            }
            _startNode.Hide();
        }
    }

    public class Node
    {
        class PossibleNeighboor
        {
            public Vec2Int offset;
            public GameObject sprite;
        }

        Vector2 _worldPosition = Vector2.zero;
        public Vector2 worldPosition {get { return _worldPosition; }}
        Vec2Int _gridPosition = Vec2Int.zero;
        public Vec2Int gridPosition { get { return _gridPosition; }}
        List<Node> _neighbors = new List<Node>();

        List<PossibleNeighboor> _possibleNeighboors = new List<PossibleNeighboor>();
        List<PossibleNeighboor> _doors = new List<PossibleNeighboor>();

        GameObject _roomSprite = null;
        GameObject _fakeRoom = null;
        Transform _canvasTransform;

        GameObject _doorSprite = null;
        

        bool drawn = false;

        public Node(Vector2 worldPosition, Vec2Int gridPosition, GameObject roomSprite, Transform canvasTransform, GameObject fakeRoom)
        {
            _fakeRoom = fakeRoom;
            _worldPosition = worldPosition;
            _gridPosition = gridPosition;
            _canvasTransform = canvasTransform;
            Vector2 centerPosition = Camera.main.WorldToScreenPoint(Camera.main.transform.position);
            _roomSprite = GameObject.Instantiate(roomSprite, canvasTransform.Find("layer1"));
            _roomSprite.SetActive(false);
        }

        public void Hide()
        {
            if (!drawn)
                return;
            _roomSprite.SetActive(false);
            drawn = false;
            for (int i = 0; i < _neighbors.Count; ++i)
            {
                _neighbors[i].Hide();
            }
            for (int i = 0; i < _possibleNeighboors.Count; ++i)
            {
                _possibleNeighboors[i].sprite.SetActive(false);
            }
            for (int i = 0; i < _doors.Count; ++i)
            {
                _doors[i].sprite.SetActive(false);
            }
        }
        public void Draw(float offset, Canvas canvas, Vec2Int roomOffset)
        {
            if (drawn)
                return;
            drawn = true;
            Vector2 centerPosition = Camera.main.WorldToScreenPoint(Camera.main.transform.position);
            _roomSprite.SetActive(true);
            Vector2 floatposition = new Vector2(gridPosition.x, gridPosition.y);
            Vector2 floatOffset = new Vector2(roomOffset.x, roomOffset.y);
            Vector2 drawPosition = centerPosition + (floatposition * offset) + (floatOffset * offset);
            _roomSprite.transform.position = drawPosition;
            //GameObject.Instantiate(roomSprite, centerPosition + (gridPosition * offset) + (roomOffset * offset), Quaternion.identity, canvas.transform);
            for (int i = 0; i < _neighbors.Count; ++i)
            {
                _neighbors[i].Draw(offset, canvas, roomOffset);
            }
            for (int i = 0; i < _possibleNeighboors.Count; ++i)
            {
                _possibleNeighboors[i].sprite.SetActive(true);
                Vec2Int calc1 = (gridPosition + _possibleNeighboors[i].offset);
                Vector2 neighboorOffsetFLoat = new Vector2(calc1.x, calc1.y);
                Vector2 floatPositionNew = centerPosition + (neighboorOffsetFLoat * offset) + (floatOffset * offset);
                _possibleNeighboors[i].sprite.transform.position = floatPositionNew;
            }
            for (int i = 0; i < _doors.Count; ++i)
            {
                _doors[i].sprite.SetActive(true);
                Vector2 floatOffsetDoors = new Vector2(_doors[i].offset.x, _doors[i].offset.y);
                floatOffsetDoors /= 2;
                Vec2Int calc1 = (gridPosition + _doors[i].offset);
                Vector2 neighboorOffsetFLoat = floatposition + floatOffsetDoors;
                //neighboorOffsetFLoat /= 2;
                Vector2 floatPositionNew = centerPosition + (neighboorOffsetFLoat * (offset)) + (floatOffset * offset);
                _doors[i].sprite.transform.position = floatPositionNew;
            }
        }

        public void AddNeighboor(Node node)
        {
            for (int i = 0; i < _possibleNeighboors.Count; ++i)
            {
                if (_possibleNeighboors[i].offset + _gridPosition == node._gridPosition)
                {
                    GameObject.Destroy(_possibleNeighboors[i].sprite);
                    _possibleNeighboors.RemoveAt(i);
                    break;
                }
            }
            for (int i = 0; i < _neighbors.Count; ++i)
            {
                if (_neighbors[i]._gridPosition == node._gridPosition)
                {
                    return;
                }
            }
            if (!_neighbors.Contains(node))
                _neighbors.Add(node);
        }

        public void AddPossibleNeighboor(Vec2Int deltaPosition, GameObject doorSprite)
        {
            _doorSprite = doorSprite;
            for (int i = 0; i < _neighbors.Count; ++i)
            {
                if (_neighbors[i]._gridPosition == _gridPosition + deltaPosition)
                {
                    return;
                }
            }
            bool exists = false;
            for (int i = 0; i < _possibleNeighboors.Count; ++i)
            {
                if (_possibleNeighboors[i].offset == deltaPosition)
                {
                    exists = true;
                    break;
                }

            }
            if (!exists)
            {
                PossibleNeighboor newPossible = new PossibleNeighboor();
                newPossible.offset = deltaPosition;

                _worldPosition = worldPosition;
                _gridPosition = gridPosition;
                Vector2 centerPosition = Camera.main.WorldToScreenPoint(Camera.main.transform.position);
                GameObject newSprite = GameObject.Instantiate(_fakeRoom, _canvasTransform.Find("layer1"));
                GameObject newDoorSprite = GameObject.Instantiate(doorSprite, _canvasTransform.Find("layer2"));
                newDoorSprite.SetActive(false);
                newSprite.SetActive(false);
                newPossible.sprite = newSprite;
                _possibleNeighboors.Add(newPossible);

                PossibleNeighboor newDoor = new PossibleNeighboor();
                newDoor.sprite = newDoorSprite;
                newDoor.offset = deltaPosition;

                _doors.Add(newDoor);
            }
        }

        public Node HasNeighboor(Vec2Int currentGrid)
        {
            for (int i = 0; i < _neighbors.Count; ++i)
            {
                if (_neighbors[i]._gridPosition == currentGrid)
                {
                    return _neighbors[i];
                }
            }
            return null;
        }

        public void CheckFake(Vec2Int currentGrid)
        {
            for (int i = 0; i < _possibleNeighboors.Count; ++i)
            {
                if (_possibleNeighboors[i].offset + gridPosition == currentGrid)
                {
                    GameObject.Destroy(_possibleNeighboors[i].sprite);
                    _possibleNeighboors.RemoveAt(i);
                }
            }
        }
    }

}