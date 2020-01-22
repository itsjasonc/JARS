using UnityEngine;

public class Barrel : Interactable {
	[SerializeField] GameObject explosion;
	private GameObject effect;
	private SphereCollider sphereCollider;
	private BoxCollider boxCollider;
	private AudioSource soundEffect;
	private Player player;
	private float startTime = 0f;
	private int explosionDamage = 10;
    private int worth;
    private int numCasualties = 0;
	
	private void Awake() {
        startTime = 0f;
        explosionDamage = 10;
        worth = 50;
	}
	
	private void Start() {
		startTime = 0f;
		boxCollider = GetComponent<BoxCollider>();
		sphereCollider = GetComponent<SphereCollider>();
		soundEffect = GetComponent<AudioSource>();
	}
	
	private void Update() {
		if (startTime > 0f) {
			if ((Time.time - startTime) >= 1f) { // 1 second explosion
                // Give the player bonus points for blowing it up
                player.incrementScore(worth * numCasualties);
                sphereCollider.enabled = false;
				if ((Time.time - startTime) >= 3f) {
					Destroy(effect);
					Destroy(gameObject);
				}
			}
		}
	}
	
	public override void onInteract(object[] objs) {
		switch (((Interactable)objs[0]).name) {
			case "Player One":
			case "Player Two":
			if (startTime == 0.0f) {
                boxCollider.enabled = false;
                sphereCollider.enabled = true;
                effect = Instantiate(explosion, transform.position, transform.rotation);
				soundEffect.Play();
				player = (Player)objs[0];
				startTime = Time.time;
				transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false;
			}
			break;
		}
	}

    private void OnCollisionEnter(Collision collision) {
        Debug.Log(collision);
        if (collision.gameObject.tag.Equals("Zombie")) {
            numCasualties++;
            object[] data = new object[3];
            data[0] = player;
            data[1] = Interactable.Type.Damage;
            data[2] = explosionDamage;
            MonoBehaviour[] scripts = collision.gameObject.GetComponents<MonoBehaviour>();
            for (int i = 0; i < scripts.Length; i++) {
                if (scripts[i].GetType().IsSubclassOf(typeof(Interactable))) {
                    collision.gameObject.SendMessage("onInteract", data);
                }
            }
        }
    }
}
