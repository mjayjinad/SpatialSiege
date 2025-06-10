using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlasmaGun : MonoBehaviour
{

    [SerializeField] private Plasma _bulletPrefab;
    [SerializeField] private Transform _spawnPos;
    [SerializeField] private TMP_Text _bulletCountTxt;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _gunShotClip;
    [SerializeField] private AudioClip _blankClip;
    [SerializeField] private float _speed;
    [SerializeField] private float _force;
    [SerializeField] private float _maxBullet;
    [SerializeField] private float _currentBulletCount;

    private void Start()
    {
        _currentBulletCount = _maxBullet;
        _bulletCountTxt.text = _currentBulletCount.ToString() + "/" + _maxBullet;
    }

    private void Update()
    {
        if (_currentBulletCount <= 0)
            _bulletCountTxt.color = Color.red;

        else
            _bulletCountTxt.color = Color.green;

        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            Shoot();
        }

        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            ReloadBullet();
        }
    }

    private void Shoot()
    {
        if (_currentBulletCount <= 0)
        {
            _audioSource.PlayOneShot(_blankClip);
            _bulletCountTxt.text = "Reload";
            return;
        }

        Plasma bullet = Instantiate(_bulletPrefab, _spawnPos.position, _spawnPos.rotation);

        bullet.Init(_spawnPos.forward * _speed);
        _audioSource.PlayOneShot(_gunShotClip);
        _currentBulletCount--;
        _bulletCountTxt.text = _currentBulletCount.ToString() + "/" + _maxBullet;
    }

    public void ReloadBullet()
    {
        if (_currentBulletCount != 0) return;
        _currentBulletCount = _maxBullet;
        _bulletCountTxt.text = _currentBulletCount.ToString() + "/" + _maxBullet;
    }
}
