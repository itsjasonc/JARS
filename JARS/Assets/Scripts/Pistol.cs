using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon {
	[SerializeField] AudioClip shotSoundFX;
	[SerializeField] AudioClip reloadSoundFX;
	[SerializeField] AudioClip dryfireSoundFX;

	void Awake () {
        weaponName = "Pistol";
		MAX_AMMO = 8;
		DAMAGE = 1;
		reloadable = true;
		FIRE_RATE = 0;
        ammoCount = MAX_AMMO;
        currentFiringMode = Weapon.FIRING_MODE.SINGLE;
        lastFired = 0;
	}
	
	void Start () {
		shotSound = shotSoundFX;
		reloadSound = reloadSoundFX;
		dryfireSound = dryfireSoundFX;
	}
}
