using System.Collections.Generic;
using UnityEngine;

public class Player : Interactable {
    public static Vector2 MAX_RETICLE_SIZE;
    public static Vector2 MIN_RETICLE_SIZE;
	[SerializeField] AudioClip hurtSound;
    private Vector2 reticleCoordinates;
    private List<Weapon> weapons;
    private int currentWeaponIndex;
    private int score;
    private int shotsFired;
    private int shotsHit;
    private int shotsWeakSpot;
    private int civiliansRescued;
    private int enemiesKilled;
    private int health;
	private int sensitivity;
    private bool inMenu;
    private const int MAX_SENSITIVITY = 100;

    private void Awake() {
        active = true;
        sensitivity = 5;
        score = 0;
        shotsFired = 0;
        shotsHit = 0;
        civiliansRescued = 0;
        health = 3;
        weapons = new List<Weapon>();
        inMenu = false;
        reticleCoordinates = new Vector2(0, 0);
        foreach (MonoBehaviour script in gameObject.GetComponents<MonoBehaviour>()) {
            switch (script.GetType().ToString()) {
                case "Pistol":
                    weapons.Add((Pistol)script);
                    break;
            }
        }
    }

    private void Start() {
    }

    public bool isInMenu() { return inMenu; }
    public void setInMenu(bool inMenu) { this.inMenu = inMenu; }

    public Vector2 getCoordinates() {
        return reticleCoordinates;
    }

    public void setCoordinates(Vector2 coordinates) {
        reticleCoordinates = coordinates;
    }

    public int getScore() {
        return score;
    }

    public void setScore(int score) {
        this.score = score;
    }

    public int getShotsFired() {
        return shotsFired;
    }

    public void setShotsFired(int shotsFired) {
        this.shotsFired = shotsFired;
    }

    public void incrementShotsFired() {
        this.shotsFired++;
    }

    public int getShotsHit() {
        return shotsHit;
    }

    public void setShotsHit(int shotsHit) {
        this.shotsHit = shotsHit;
    }

    public void incrementShotsHit() {
        this.shotsHit++;
    }

    public int getShotsWeakSpot() {
        return shotsWeakSpot;
    }

    public void setShotsWeakSpot(int shotsWeakSpot) {
        this.shotsWeakSpot = shotsWeakSpot;
    }

    public void incrementShotsWeakSpot() {
        this.shotsWeakSpot++;
    }

    public int getCiviliansRescued() {
        return civiliansRescued;
    }

    public void setCiviliansRescued(int rescued) {
        civiliansRescued = rescued;
    }

    public int getEnemiesKilled() {
        return enemiesKilled;
    }

    public void setEnemiesKilled(int killed) {
        enemiesKilled = killed;
    }

    public void incrementEnemiesKilled() {
        this.enemiesKilled++;
    }

    public int getHealth() {
        return health;
    }

    public void setHealth(int health) {
        this.health = health;
    }
	
	public void increaseHealth(int health) {
		this.health += health;
	}

    public Weapon getCurrentWeapon() {
        if (weapons.Count > 0) {
            if (currentWeaponIndex > weapons.Count)
                currentWeaponIndex = 0;
            return weapons[currentWeaponIndex];
        }
        return null;
    }

    public bool hasWeapon() {
        if (weapons.Count > 0) {
            return false;
        }
        return true;
    }
	
	public int getSensitivity() { return sensitivity; }
	public void incrementSensitivity() { if (sensitivity < MAX_SENSITIVITY) sensitivity++; }
	public void decrementSensitivity() { if (sensitivity > 1) sensitivity--; }
	public void setSensitivity(int sensitivity) { this.sensitivity = sensitivity; }

    /**
     * Fires currently equipped weapon
     */
    public bool interact() {
        if (currentWeaponIndex > weapons.Count)
            currentWeaponIndex = 0;
        if (getCurrentWeapon().getAmmoCount() == 0) {
            AudioSource.PlayClipAtPoint(getCurrentWeapon().getDryFireSound(), transform.position);
            return false;
        } else {
            if (getCurrentWeapon().getLastFired() <= (Time.time - getCurrentWeapon().getFiringRate())) {
                getCurrentWeapon().setLastFired(Time.time);
                AudioSource.PlayClipAtPoint(getCurrentWeapon().getShotSound(), transform.position);
                if (!inMenu && active) {
                    incrementShotsFired();
                    getCurrentWeapon().setAmmoCount(getCurrentWeapon().getAmmoCount() - 1);
                }
            }
        }
        return true;
    }

    /**
     * Reloads the currently equipped weapon
     */
    public void reload() {
        if (currentWeaponIndex > weapons.Count)
            currentWeaponIndex = 0;
        // The weapon is not reloadable, we should probably get rid of it
        if (!getCurrentWeapon().isReloadable()) {
            weapons.RemoveAt(currentWeaponIndex);
            currentWeaponIndex = 0;
            return;
        }
        AudioSource.PlayClipAtPoint(getCurrentWeapon().getReloadSound(), transform.position);
        getCurrentWeapon().setAmmoCount(getCurrentWeapon().getMaxAmmoCount());
    }

    public void incrementScore(int score) {
        this.score += score;
    }
	
	public override void onInteract(object[] objs) {
		if (health > 0) {
			if (((Interactable.Type)objs[1]) == Interactable.Type.Damage) {
				health -= ((int)objs[2]);
                AudioSource.PlayClipAtPoint(hurtSound, transform.position);
			}
		}
		if (health <= 0) {
			health = 0;
			active = false;
		}
	}
}
