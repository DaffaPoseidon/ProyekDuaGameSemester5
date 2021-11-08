using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PermanentUI : MonoBehaviour
{
    //Status Player
    public int ceri = 0;
    public int health;
    public TextMeshProUGUI teksCeri;
    public Text healthAmount;

    public static PermanentUI perm;

    private void Start()
    {
        //Perintah agar Objek Game tidak dihapus saat berganti level
        DontDestroyOnLoad(gameObject);

        //Singleton
        if (!perm)
        {
            perm = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Reset()
    {
        ceri = 0;
        teksCeri.text = ceri.ToString();
    }
}
