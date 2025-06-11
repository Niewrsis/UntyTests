using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerMovementTests
{
    private GameObject playerObj;
    private PlayerController player;
    private Rigidbody2D rb;

    [SetUp]
    public void Setup()
    {
        playerObj = new GameObject("Player");
        rb = playerObj.AddComponent<Rigidbody2D>();
        playerObj.AddComponent<BoxCollider2D>();
        player = playerObj.AddComponent<PlayerController>();

        rb.gravityScale = 0;
        playerObj.layer = LayerMask.NameToLayer("Default");
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(playerObj); // �������� Destroy �� DestroyImmediate
    }

    [Test]
    public void PlayerMovesRight_WhenGivenPositiveInput()
    {
        player.Move(1f);
        Assert.Greater(player.CurrentHorizontalSpeed, 0f);
    }

    [Test]
    public void PlayerMovesLeft_WhenGivenNegativeInput()
    {
        player.Move(-1f);
        Assert.Less(player.CurrentHorizontalSpeed, 0f);
    }

    [UnityTest]
    public IEnumerator PlayerJumps_WhenJumpInputAndGrounded()
    {
        rb.gravityScale = 1;

        var ground = new GameObject("Ground");
        ground.AddComponent<BoxCollider2D>();
        ground.layer = LayerMask.NameToLayer("Ground"); // �������, ��� ���� ��������� � groundLayer � PlayerController
        ground.transform.position = playerObj.transform.position - new Vector3(0, 1f, 0);

        yield return null; // � Edit Mode ����� ������ yield return null

        player.Jump();
        float initialYVelocity = rb.velocity.y;

        Assert.Greater(initialYVelocity, 0f);
        Object.DestroyImmediate(ground); // �������� Destroy
    }

    [Test]
    public void PlayerDoesNotMove_WhenNoInput()
    {
        player.Move(0f);
        Assert.AreEqual(0f, player.CurrentHorizontalSpeed, 0.01f);
    }
}