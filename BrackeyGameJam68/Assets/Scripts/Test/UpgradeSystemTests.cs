using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSystemTests
{
    [Test]
    public void TestNoDuplicatePerk()
    {
        List<int> upgAvailable = new List<int>() {1,2,3,4,5};
        List<int> selected = new List<int>();
        List<int> temp = new List<int>(upgAvailable);

        for(int i = 0; i < 3; i++)
        {
            int idx = Random.Range(0, temp.Count);
            selected.Add(temp[idx]);
            temp.RemoveAt(idx);
        }

        HashSet<int> check = new HashSet<int>(selected);
        Assert.AreEqual(selected.Count, check.Count); // ถ้าไม่ซ้ำ จะผ่าน
    }
}
