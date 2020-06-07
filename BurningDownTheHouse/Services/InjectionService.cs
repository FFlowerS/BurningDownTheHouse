﻿using Anamnesis;
using Anamnesis.Offsets;
using Anamnesis.Process;
using ConceptMatrix;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BurningDownTheHouse.Services
{
	public class InjectionService : IInjectionService
	{
		private AnamnesisService service;

		public string GamePath { get => this.service.GamePath; }

		public async Task Initialize()
		{
			this.service = new AnamnesisService();
			this.service.ErrorCallback += this.OnError;
			this.service.LogCallback += this.OnLog;
			this.service.SelectProcessCallback += this.OnSelectProcess;

			await this.service.Initialize<WinProcess>();
		}

		public async Task Shutdown()
		{
			await this.service.Shutdown();
		}

		public Task Start()
		{
			return Task.CompletedTask;
		}

		private void OnError(Exception ex)
		{
			Log.Write(ex, "Injection");
		}

		private void OnLog(string message)
		{
			Log.Write(message, "Injection", Log.Severity.Log);
		}

		private async Task<Process> OnSelectProcess()
		{
			return await App.Current.Dispatcher.InvokeAsync<Process>(() =>
			{
				Log.Write("Oh shit...");
				return null;
			});
		}

		#region GetMemory Implementations

		public IMemory<T> GetMemory<T>(IBaseMemoryOffset baseOffset, params IMemoryOffset[] offsets)
		{
			return this.service.GetMemory<T>(baseOffset, offsets);
		}

		public IMemory<T> GetMemory<T>(IBaseMemoryOffset baseOffset, params IMemoryOffset<T>[] offsets)
		{
			return this.service.GetMemory<T>(baseOffset, offsets);
		}

		public IMemory<T> GetMemory<T>(IBaseMemoryOffset<T> baseOffset, params IMemoryOffset<T>[] offsets)
		{
			return this.service.GetMemory<T>(baseOffset, offsets);
		}

		#endregion
	}
}