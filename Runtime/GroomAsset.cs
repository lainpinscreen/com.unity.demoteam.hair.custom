﻿using System;
using UnityEngine;
using UnityEngine.Formats.Alembic.Importer;

namespace Unity.DemoTeam.Hair
{
	[PreferBinarySerialization, CreateAssetMenu]
	public class GroomAsset : ScriptableObject
	{
		public enum Type
		{
			Procedural,
			Alembic,
		}

		public enum MemoryLayout
		{
			Interleaved,
			Sequential,
		}

		[Serializable]
		public struct SettingsBasic
		{
			[Tooltip("Type of generator")]
			public Type type;
			[Tooltip("Memory layout for the generated strands")]
			public MemoryLayout memoryLayout;
			[Tooltip("Material applied to the generated strand groups")]
			public Material material;

			public static readonly SettingsBasic defaults = new SettingsBasic()
			{
				type = Type.Procedural,
				memoryLayout = MemoryLayout.Interleaved,
			};
		}

		[Serializable]
		public struct SettingsAlembic
		{
			[Tooltip("Alembic asset containing at least one set of curves")]
			public AlembicStreamPlayer sourceAsset;
			[Tooltip("Resample curves to ensure a specific number of particles along each strand")]
			public bool resampleCurves;//TODO
			[Range(3, HairSim.MAX_STRAND_PARTICLE_COUNT), Tooltip("Number of particles along each strand")]
			public int resampleParticleCount;//TODO

			public static readonly SettingsAlembic defaults = new SettingsAlembic()
			{
				resampleCurves = true,
				resampleParticleCount = 32,
			};
		}

		[Serializable]
		public struct SettingsProcedural
		{
			public enum Style
			{
				Curtain,
				Brush,
				Cap,
				StratifiedCurtain,
			}

			public Style style;
			[Range(64, HairSim.MAX_STRAND_COUNT), Tooltip("Number of strands")]
			public int strandCount;
			[Range(3, HairSim.MAX_STRAND_PARTICLE_COUNT), Tooltip("Number of particles along each strand")]
			public int strandParticleCount;
			[Range(0.001f, 5.0f), Tooltip("Strand length in meters")]
			public float strandLength;

			public static readonly SettingsProcedural defaults = new SettingsProcedural()
			{
				style = Style.Curtain,
				strandCount = 64,
				strandParticleCount = 32,
				strandLength = 0.25f,
			};
		}

		[Serializable]
		public struct StrandGroup
		{
			public int strandCount;
			public int strandParticleCount;

			public float maxStrandLength;
			public float maxParticleInterval;

			[HideInInspector] public float[] rootScale;
			[HideInInspector] public Vector3[] rootPosition;
			[HideInInspector] public Vector3[] rootDirection;

			[HideInInspector] public Vector3[] particlePosition;
			[HideInInspector] public MemoryLayout particleMemoryLayout;

			[HideInInspector] public Mesh meshAssetLines;
			[HideInInspector] public Mesh meshAssetRoots;
		}

		public Material defaultMaterial;

		public SettingsBasic settingsBasic = SettingsBasic.defaults;
		public SettingsAlembic settingsAlembic = SettingsAlembic.defaults;
		public SettingsProcedural settingsProcedural = SettingsProcedural.defaults;

		public StrandGroup[] strandGroups;
		public bool strandGroupsAutoBuild;

		public string checksum;

		public HairSim.SolverSettings settingsSolver = HairSim.SolverSettings.defaults;
		public HairSim.VolumeSettings settingsVolume = HairSim.VolumeSettings.defaults;
	}
}
