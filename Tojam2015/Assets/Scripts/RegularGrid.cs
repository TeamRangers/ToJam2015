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
        
        // Bilinear interpolation
        Vector2 q11, q12, q21, q22;
        q11 = new Vector2( x      * _basis.x,  y      * _basis.y);
        q21 = new Vector2((x + 1) * _basis.x,  y      * _basis.y);
        q12 = new Vector2( x      * _basis.x, (y + 1) * _basis.y);
        q22 = new Vector2((x + 1) * _basis.x, (y + 1) * _basis.y);

        Vector2 fq11, fq12, fq21, fq22;
        fq11 = _data[ x      +  y      * _xDivs];
        fq21 = x < _xDivs + 1 ? _data[(x + 1) +  y      * _xDivs] : Vector2.zero;
        fq12 = y < _yDivs + 1 ? _data[ x      + (y + 1) * _xDivs] : Vector2.zero;
        fq22 = x < _xDivs + 1 || y < _yDivs + 1 ? _data[(x + 1) + (y + 1) * _xDivs] : Vector2.zero;

        Vector2 fxy1 = (q22.x - point.x) / (q22.x - q11.x) * fq11 + (point.x - q11.x) / (q22.x - q11.x) * fq21;
        Vector2 fxy2 = (q22.x - point.x) / (q22.x - q11.x) * fq12 + (point.x - q11.x) / (q22.x - q11.x) * fq22;
        Vector2 fxy  = (q22.y - point.y) / (q22.y - q11.y) * fxy1 + (point.y - q11.y) / (q22.y - q11.y) * fxy2;

        return fxy;
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
