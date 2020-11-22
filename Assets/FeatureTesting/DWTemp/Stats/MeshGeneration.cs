using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class MeshGeneration : MonoBehaviour {
    [SerializeField] private List<Stat> values;
    [SerializeField] private Material chartMaterial;
    [SerializeField] private CanvasRenderer canvasRenderer;

    private Mesh _mesh;
    private Vector3[] _vertices;
    private Vector3[] _fullVertices;
    private Vector2[] _uv;
    private int[] _triangles;

    private bool _isAnimating;
    public float AnimationFraction;

    void Start() {
        GenerateMesh();
    }
    private void Update() {
        AnimateMesh();
    }

    private void GenerateMesh() {

        int count = values.Count;

        if (count > 0) {
            _mesh = new Mesh();

            _vertices = new Vector3[count + 1];
            _uv = new Vector2[count + 1];
            _triangles = new int[3 * count];

            float angleIncrement = 360f / count;
            float radarChartSize = 145f;


            // uv
            _uv[0] = Vector2.zero;
            for (int i = 0; i < count; i++) {
                _uv[i + 1] = Vector2.one;
            }

            // vertices and tris
            _vertices[0] = Vector3.zero;
            for (int i = 0; i < count; i++) {
                _vertices[i + 1] = Quaternion.Euler(0, 0, -angleIncrement * i) * Vector3.up * radarChartSize * values[i].GetFraction;

                // Tris
                _triangles[i * 3] = 0;
                for (int j = 0; j < 2; j++) {
                    _triangles[i * 3 + j + 1] = i + j + 1;
                }
            }
            _triangles[3 * count - 1] = 1;
            _fullVertices = new Vector3[count + 1];
            Array.Copy(_vertices, _fullVertices, _vertices.Length);

            _mesh.vertices = _vertices;
            _mesh.uv = _uv;
            _mesh.triangles = _triangles;

            canvasRenderer.SetMesh(_mesh);
            canvasRenderer.SetMaterial(chartMaterial, null);
        }
    }

    private void AnimateMesh() {
        for (int i = 0; i < _vertices.Length; i++) {
            _vertices[i] = _fullVertices[i] * AnimationFraction;
            //_vertices[i] = _fullVertices[i];
            //_vertices[i] += Vector3.up * Time.deltaTime;
        }
        _mesh.vertices = _vertices;
        _mesh.RecalculateBounds();
        canvasRenderer.SetMesh(_mesh);
    }

    private void OnValidate() {
        GenerateMesh();
        //AnimateMesh();
    }
}