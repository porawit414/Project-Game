using UnityEngine;

public class PlayerLoadPosition : MonoBehaviour
{
    [Header("ลากตัวละคร Player มาใส่ช่องนี้")]
    public GameObject playerObject;

    void Start()
    {
        // เช็คก่อนว่า ในเครื่องเคยมีการเซฟตำแหน่ง X ไว้หรือเปล่า? (ถ้าไม่มีแสดงว่าเพิ่งเล่นครั้งแรก)
        if (PlayerPrefs.HasKey("SavedPlayerX"))
        {
            // ดึงค่า X, Y, Z ออกมาจากที่เซฟไว้
            float posX = PlayerPrefs.GetFloat("SavedPlayerX");
            float posY = PlayerPrefs.GetFloat("SavedPlayerY");
            float posZ = PlayerPrefs.GetFloat("SavedPlayerZ");

            // 🌟 ทริคสำคัญของ Unity: ถ้าใช้ Character Controller ต้องปิดมันก่อนวาร์ป ไม่งั้นมันจะเด้งกลับที่เดิม
            CharacterController cc = playerObject.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            // วาร์ปผู้เล่นไปจุดที่เซฟไว้
            playerObject.transform.position = new Vector3(posX, posY, posZ);

            // เปิด Controller กลับมาให้เดินได้ปกติ
            if (cc != null) cc.enabled = true;

            Debug.Log("🔄 โหลดตำแหน่งเซฟเรียบร้อย!");
        }
    }

    // เอาไว้ใช้ตอนทำปุ่ม "New Game" เพื่อลบเซฟเก่าทิ้ง
    public void DeleteSave()
    {
        PlayerPrefs.DeleteKey("SavedPlayerX");
        PlayerPrefs.DeleteKey("SavedPlayerY");
        PlayerPrefs.DeleteKey("SavedPlayerZ");
        Debug.Log("🗑️ ลบเซฟทิ้งแล้ว เริ่มเกมใหม่!");
    }
}