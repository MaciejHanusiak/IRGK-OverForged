using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;

    [SerializeField] private GameObject[] visualByPlayerIndex = new GameObject[2];

    private readonly HashSet<Player> boundPlayers = new HashSet<Player>();

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        var p = sender as Player;
        if (p == null) return;

        int idx = p.PlayerIndex;
        if (idx < 0 || idx >= visualByPlayerIndex.Length) return;

        var vis = visualByPlayerIndex[idx];
        if (vis == null) return;

        vis.SetActive(e.selectedCounter == baseCounter);
    }

    public void Bind(Player player)
    {
        if (player == null) return;
        if (boundPlayers.Contains(player)) return;

        boundPlayers.Add(player);
        player.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;

        int idx = player.PlayerIndex;
        if (idx >= 0 && idx < visualByPlayerIndex.Length && visualByPlayerIndex[idx] != null)
            visualByPlayerIndex[idx].SetActive(false);
    }

    private void OnDestroy()
    {
        foreach (var p in boundPlayers)
        {
            if (p != null)
                p.OnSelectedCounterChanged -= Player_OnSelectedCounterChanged;
        }
        boundPlayers.Clear();
    }
}
