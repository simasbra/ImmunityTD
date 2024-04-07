using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TowerSelectionButtonTests
{

    private GameObject towerSelectionButtonObject;
    private TowerSelectionButton towerSelectionButton;

    [TearDown]
    public void TearDown()
    {
        // Clean up
        GameObject.DestroyImmediate(towerSelectionButton);
        GameObject.DestroyImmediate(towerSelectionButtonObject);
    }

    [Test]
    public void TowerPrefabNotAssigned_ErrorLogged()
    {
        // Arrange
        GameObject towerSelectionButtonObject = new GameObject();
        TowerSelectionButton towerSelectionButton = towerSelectionButtonObject.AddComponent<TowerSelectionButton>();

        // Act
        towerSelectionButton.Start();

        // Assert
        LogAssert.Expect(LogType.Error, "Tower prefab is not assigned to the tower selection button!");
    }
    [UnityTest]
    public IEnumerator PlaceTower_CurrentSlotNull_NoTowerPlaced()
    {
        // Arrange
        GameObject towerSelectionButtonObject = new GameObject();
        TowerSelectionButton towerSelectionButton = towerSelectionButtonObject.AddComponent<TowerSelectionButton>();
        TowerSelectionButton.currentSlot = null; // Set currentSlot to null
        towerSelectionButton.towerPrefab = new GameObject();
        Player.coins = 20; // Set coins to 20
        towerSelectionButton.price = 10f; // Set price

        // Act
        towerSelectionButton.Place();
        yield return null; // Wait for a frame to process Unity's warning

        // Assert
        LogAssert.Expect(LogType.Warning, "No slot selected.");
        Assert.AreEqual(20, Player.coins); // Coins remain unchanged
    }

    [UnityTest]
    public IEnumerator PlaceTower_NotEnoughCoins_NoTowerPlaced()
    {
        GameObject towerSelectionButtonObject = new GameObject();
        TowerSelectionButton towerSelectionButton = towerSelectionButtonObject.AddComponent<TowerSelectionButton>();
        towerSelectionButton.towerPrefab = new GameObject();
        TowerSelectionButton.currentSlot = towerSelectionButtonObject.AddComponent<Slot>(); // Mock currentSlot

        TowerSelectionButton.currentSlot.clicked = true;
        Player.coins = 0; // Set coins to 0
        towerSelectionButton.price = 10f; // Set price

        // Act
        towerSelectionButton.Place();
        yield return null; // Wait for a frame to process Unity's warning

        // Assert
        LogAssert.Expect(LogType.Warning, "Not enough coins.");
        Assert.IsNotNull(TowerSelectionButton.currentSlot); // Ensure no tower is placed
    }

    [UnityTest]
    public IEnumerator PlaceTower_EnoughCoins_TowerPlaced()
    {
        GameObject towerSelectionButtonObject = new GameObject();
        TowerSelectionButton towerSelectionButton = towerSelectionButtonObject.AddComponent<TowerSelectionButton>();
        TowerSelectionButton.currentSlot = towerSelectionButtonObject.AddComponent<Slot>(); // Mock currentSlot
        towerSelectionButton.towerPrefab = new GameObject();


        TowerSelectionButton.currentSlot.clicked = true;
        Player.coins = 20; // Set coins to 20
        towerSelectionButton.price = 10f; // Set price

        // Act
        towerSelectionButton.Place();
        yield return null; // Wait for a frame to process Unity's warning

        // Assert
        Assert.AreEqual(10, Player.coins); // Coins deducted correctly
        Assert.IsNull(TowerSelectionButton.currentSlot); // Tower placed
    }
    [UnityTest]
    public IEnumerator OnTowerButtonClick_PurchaseButtonThis()
    {
        GameObject towerSelectionButtonObject = new GameObject();
        TowerSelectionButton towerSelectionButton = towerSelectionButtonObject.AddComponent<TowerSelectionButton>();
        towerSelectionButton.towerPrefab = new GameObject();

        towerSelectionButton.OnTowerButtonClick();
        yield return null; // Wait for a frame to process Unity's warning

        // Assert
        Assert.AreEqual(towerSelectionButton, PurchaseButton.towerButton);
    }
    [UnityTest]
    public IEnumerator Place_Slot_IsClicked()
    {
        GameObject towerSelectionButtonObject = new GameObject();
        TowerSelectionButton towerSelectionButton = towerSelectionButtonObject.AddComponent<TowerSelectionButton>();
        towerSelectionButton.towerPrefab = new GameObject();

        TowerSelectionButton.currentSlot = towerSelectionButtonObject.AddComponent<Slot>(); // Mock currentSlot


        towerSelectionButton.Place();
        yield return null; // Wait for a frame to process Unity's warning

        LogAssert.Expect(LogType.Warning, "Slot is not clicked.");
    }
}