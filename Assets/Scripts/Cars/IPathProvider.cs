using System.Collections.Generic;
using UnityEngine;

public interface IPathProvider
{
    IEnumerable<Vector3> GetPoints();
}