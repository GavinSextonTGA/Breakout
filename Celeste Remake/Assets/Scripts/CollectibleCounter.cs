using UnityEngine;
using TMPro;

public class CollectibleCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private void Update()
    {
        _text.text = Collectible.count.ToString();
    }
}
