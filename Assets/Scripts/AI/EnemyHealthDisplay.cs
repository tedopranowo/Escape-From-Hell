using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthDisplay : MonoBehaviour {
    [SerializeField]
    GameObject _healthBarPrefab = null;
    GameObject _myHealthbar = null;
    [SerializeField]
    Vector2 _healthbarOffset = Vector2.zero;
    Vector3 _healthbarOffset3D = Vector3.zero;
    RectTransform _healthbarRect = null;
    Camera _usedCamera = null;
    [SerializeField]
    Character _myCharacter = null;

    Vector2 _healthbarSize = Vector2.zero;
    Vector2 _healthSize = Vector2.zero;
    [SerializeField]
    GameObject _enemyUiPrefab = null;
    [SerializeField]
    Color _fullColor;
    [SerializeField]
    Color _emptyColor;

    Image _healthbarImage; 
    // Use this for initialization
    void Start () {
        GameObject enemyUi = GameObject.FindGameObjectWithTag("EnemyUI");
        if (!enemyUi)
        {
            if (!_enemyUiPrefab)
                return;
            enemyUi = Instantiate(_enemyUiPrefab);
        }
        _usedCamera = Camera.main;
        _healthbarOffset3D = _healthbarOffset;
        _myHealthbar = Instantiate(_healthBarPrefab, _usedCamera.WorldToScreenPoint(transform.position + _healthbarOffset3D), Quaternion.identity, enemyUi.transform);
        _healthbarRect = _myHealthbar.GetComponent<RectTransform>();
        _healthbarImage = _myHealthbar.GetComponent<Image>();
        _healthbarSize.y = _healthbarRect.sizeDelta.x;
        _healthSize.y = _myCharacter.maxHealth;
    }

	
	// Update is called once per frame
	void Update () {
        if (!_myHealthbar)
            return;
        _healthbarRect.position = _usedCamera.WorldToScreenPoint(transform.position + _healthbarOffset3D);
        float currentValueCalculated = (_myCharacter.health - _healthSize.x) / (_healthSize.y - _healthSize.x) * (_healthbarSize.y - _healthbarSize.x) + _healthbarSize.x;
        _healthbarRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentValueCalculated);
        float calculatedValueNormalized = (_myCharacter.health - _healthSize.x) / (_healthSize.y - _healthSize.x) * (1 - 0) + 0;
        _healthbarImage.color = Color.Lerp(_emptyColor, _fullColor, calculatedValueNormalized);
    }

    private void OnDisable()
    {
        if(_myHealthbar)
        _myHealthbar.SetActive(false);
    }

    private void OnEnable()
    {
        if(_myHealthbar)
        _myHealthbar.SetActive(true);
    }

    private void OnDestroy()
    {
        if (!_myHealthbar)
            return;
        Destroy(_myHealthbar);
    }
}
