using UnityEngine;

public class Weapon : MonoBehaviour {
	public enum FIRING_MODE {
		SINGLE,
		AUTOMATIC
	}
	
	protected AudioClip shotSound;
	protected AudioClip reloadSound;
	protected AudioClip dryfireSound;
	protected string weaponName;
	protected int ammoCount;
	protected int MAX_AMMO;
	protected int DAMAGE;
	protected bool reloadable;
	protected FIRING_MODE currentFiringMode;
	protected int FIRE_RATE;
	protected float lastFired;
	
	public virtual string getName() { return weaponName; }
	public virtual int getAmmoCount() { return ammoCount; }
	public virtual void setAmmoCount(int ammoCount) { this.ammoCount = ammoCount; }
	public virtual int getMaxAmmoCount() { return MAX_AMMO; }
	public virtual int getDamage() { return DAMAGE; }
	public virtual bool isReloadable() { return reloadable; }
	public virtual FIRING_MODE getFiringMode() { return currentFiringMode; }
	public virtual void setFiringMode(FIRING_MODE firingMode) { currentFiringMode = firingMode; }
	public virtual int getFiringRate() { return FIRE_RATE; }
	public virtual float getLastFired() { return lastFired; }
	public virtual void setLastFired(float lastFired) { this.lastFired = lastFired; }
	public virtual AudioClip getShotSound() { return shotSound; }
	public virtual AudioClip getReloadSound() { return reloadSound; }
	public virtual AudioClip getDryFireSound() { return dryfireSound; }
}