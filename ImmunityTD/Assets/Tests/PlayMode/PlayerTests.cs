using NUnit.Framework;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.TestTools;
using Assets.Scripts;

public class PlayerTests
{
    private Player player;
    private GameObject enemyGenerator;
    private TextMeshProUGUI coinsText;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI killsText;
    private TextMeshProUGUI enemySpawnDelayText;

    [SetUp]
    public void SetUp()
    {
        GameObject playerGO = new GameObject("Player");
        player = playerGO.AddComponent<Player>();
        enemyGenerator = new GameObject("EnemyGenerator");
        player.EnemyGenerator = enemyGenerator;

        coinsText = new GameObject("CoinsText").AddComponent<TextMeshProUGUI>();
        scoreText = new GameObject("ScoreText").AddComponent<TextMeshProUGUI>();
        killsText = new GameObject("KillsText").AddComponent<TextMeshProUGUI>();
        enemySpawnDelayText = new GameObject("EnemySpawnDelayText").AddComponent<TextMeshProUGUI>();
        player.CoinsText = coinsText;
        player.ScoreText = scoreText;
        player.KillsText = killsText;
        player.EnemySpawnDelayText = enemySpawnDelayText;

        Player.Coins = 100f;
        Player.Score = 0;
        Player.Kills = 0;
    }

    [Test]
    public void InitialState_IsCorrect()
    {
        Assert.AreEqual(100f, Player.Coins);
        Assert.AreEqual(0, Player.Score);
        Assert.AreEqual(0, Player.Kills);
    }

    [Test]
    public void AddCoins_IncreasesCoins()
    {
        Player.AddCoins(50f);
        Assert.AreEqual(150f, Player.Coins);
    }

    [Test]
    public void AddScore_IncreasesScore()
    {
        Player.AddScore(10);
        Assert.AreEqual(10, Player.Score);
    }

    [Test]
    public void AddKill_IncreasesKills()
    {
        Player.AddKill();
        Assert.AreEqual(1, Player.Kills);
    }

    [Test]
    public void AddKills_IncreasesMultipleKills()
    {
        Player.AddKills(5);
        Assert.AreEqual(5, Player.Kills);
    }

    [Test]
    public void FixedUpdate_UpdatesUIElements()
    {
        for (int i = 0; i < 5; i++)
        {
            player.FixedUpdate();
        }

        Assert.AreEqual("100", coinsText.text);
        Assert.AreEqual("0", scoreText.text);
        Assert.AreEqual("0", killsText.text);
    }

    [UnityTest]
    public IEnumerator FixedUpdate_ActivatesEnemyGeneratorAfterDelay()
    {
        float simulateTime = player.GeneratorDelay + 1f;
        for (float t = 0; t < simulateTime; t += Time.fixedDeltaTime)
        {
            yield return new WaitForFixedUpdate();
        }
        Assert.IsTrue(enemyGenerator.activeSelf, "Enemy generator should be active after the delay.");
        Assert.IsFalse(enemySpawnDelayText.enabled, "Enemy spawn delay text should be disabled after the delay.");
    }

    [TearDown]
    public void TearDown()
    {
        GameObject.DestroyImmediate(player.gameObject);
        GameObject.DestroyImmediate(enemyGenerator);
        GameObject.DestroyImmediate(coinsText.gameObject);
        GameObject.DestroyImmediate(scoreText.gameObject);
        GameObject.DestroyImmediate(killsText.gameObject);
        GameObject.DestroyImmediate(enemySpawnDelayText.gameObject);

        Player.Coins = 100f;
        Player.Score = 0;
        Player.Kills = 0;
    }
}
