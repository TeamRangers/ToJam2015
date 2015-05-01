using UnityEngine;
using System.Collections;

public class RegularGrid {

    float _width;
    float _height;
    int _xDivs;
    int _yDivs;
    Vector2 _basis;

    Vector2[] _data;

    public RegularGrid(float width, float height, int xDivs, int yDivs)
    {
        _data = new Vector2[(xDivs + 1) * (yDivs + 1)];
        _basis = new Vector2(width / (float) xDivs, height / (float) yDivs);
        _width = width;
        _height = height;
        _xDivs = xDivs;
        _yDivs = yDivs;
    }

    public Vector2 GetValue(Vector2 point)
    {
        int x = Mathf.FloorToInt(point.x / _basis.x);
        int y = Mathf.FloorToInt(point.y / _basis.y);

        if (x < 0 || x >= _xDivs + 1 || y < 0 || y >= _yDivs + 1)
        {
            throw new UnityException("location out of bounds");
        }

        return _data[x + y * _xDivs];
    }

    public void SetValue(Vector2 point, Vector2 value)
    {
        int x = Mathf.FloorToInt(point.x / _basis.x);
        int y = Mathf.FloorToInt(point.y / _basis.y);

        if (x < 0 || x >= _xDivs + 1 || y < 0 || y >= _yDivs + 1)
        {
            throw new UnityException("location out of bounds");
        }

        _data[x + y * _xDivs] = value;
    }

    public Vector2[] GetPoints()
    {
        Vector2[] points = new Vector2[(_xDivs + 1) * (_yDivs + 1)];

        for (int y = 0; y < _yDivs + 1; ++y)
        {
            for (int x = 0; x < _xDivs + 1; ++x) {
                points[x + y * _xDivs] = new Vector2(x * _basis.x, y * _basis.y); 
            }
        }

        return points;
    }
    
}
