using UnityEngine;

[CreateAssetMenu(fileName = "New CharacterData", menuName = "Characters/Character", order = 0)]
public class CharacterData : ScriptableObject
{
    [SerializeField] private string _characterName;
    [SerializeField] private Sprite _skin;
    [SerializeField] private float _hitPoints;
    [SerializeField] private float _walkSpeed;
}