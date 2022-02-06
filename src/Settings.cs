using System;
using System.Collections.Generic;
using System.Reflection;
using Modding;
using UnityEngine;

namespace EnemyHPBar
{
	[Serializable]
	public class Settings
	{
		public float fgScale = 1.0f;
		public float bgScale = 1.0f;
		public float mgScale = 1.0f;
		public float olScale = 1.0f;
		public float bossfgScale = 1.0f;
		public float bossbgScale = 1.0f;
		public float bossolScale = 1.0f;
		public int NameLength = 10;
		public string DefaultSkin { get; set; } = "Default";
	}
}