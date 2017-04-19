using UnityEngine;

namespace Assets.Scripts
{
    public class Spawner : MonoBehaviour {

        // Groups
        public GameObject[] Groups;
        // Use this for initialization
        void Start () {
            // Spawn initial Group
            SpawnNext();
        }
	
        // Update is called once per frame
        void Update () {
		
        }
        public void SpawnNext()
        {
            // Random Index
            var index = Random.Range(0, Groups.Length);

            // Spawn Group at current Position
            Instantiate(Groups[index],
                transform.position,
                Quaternion.identity);
        }
    }
}
