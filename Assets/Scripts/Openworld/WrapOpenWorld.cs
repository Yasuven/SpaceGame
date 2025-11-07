using UnityEngine;

public class WorldWrap : MonoBehaviour
{
    public Transform leftWall;
    public Transform rightWall;
    public Transform topWall;
    public Transform bottomWall;
    public float offset = 0.5f;

    private float _leftBound;
    private float _rightBound;
    private float _topBound;
    private float _bottomBound;

    void Start()
    {
        _leftBound = leftWall.position.x;
        _rightBound = rightWall.position.x;
        _topBound = topWall.position.y;
        _bottomBound = bottomWall.position.y;
    }

    void FixedUpdate()
    {
        Vector3 pos = transform.position;
        bool wrapped = false;

        // horizontal
        if (pos.x > _rightBound)
        {
            pos.x = _leftBound + offset;
            wrapped = true;
        }
        else if (pos.x < _leftBound)
        {
            pos.x = _rightBound - offset;
            wrapped = true;
        }

        // vertical
        if (pos.y > _topBound)
        {
            pos.y = _bottomBound + offset;
            wrapped = true;
        }
        else if (pos.y < _bottomBound)
        {
            pos.y = _topBound - offset;
            wrapped = true;
        }

        if (wrapped)
        {
            transform.position = pos;
        }
    }
}