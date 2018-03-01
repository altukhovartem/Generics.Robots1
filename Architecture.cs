using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Generics.Robots
{

	public interface IRobotic<out T>
	{
		T GetCommand();
	}

	public abstract class RobotAI<T> : IRobotic<T>
	{
		public abstract T GetCommand();
	}

	public class ShooterAI<T> : RobotAI<T>
		where T : ShooterCommand
	{
		int counter = 1;

		public override T GetCommand()
		{
			return ShooterCommand.ForCounter(counter++) as T;
		}
	}

	public class BuilderAI<T> : RobotAI<T>
				where T : BuilderCommand
	{
		int counter = 1;
		public override T GetCommand()
		{
			return BuilderCommand.ForCounter(counter++) as T;
		}
	}

	public interface IDevicable<in T>
	{
		string ExecuteCommand(T command);
	}

	public abstract class Device<T> : IDevicable<T>
	{
		public abstract string ExecuteCommand(T command);
	}

	public class Mover<T> : Device<T>
	{
		public override string ExecuteCommand(T _command)
		{
			var command = _command as IMoveCommand;
			if (command == null)
				throw new ArgumentException();
			return $"MOV {command.Destination.X}, {command.Destination.Y}";
		}
	}



	public class Robot<T>
	{
		RobotAI<T> ai;
		Device<T> device;

		public Robot(RobotAI<T> ai, Device<T> executor)
		{
			this.ai = ai;
			this.device = executor;
		}

		public IEnumerable<string> Start(int steps)
		{
			for (int i = 0; i < steps; i++)
			{
				var command = ai.GetCommand();
				if (command == null)
					break;
				yield return device.ExecuteCommand(command);
			}

		}

		public static Robot<T> Create(RobotAI<T> ai, Device<T> executor)
		{
			return new Robot<T>(ai, executor);
		}
	}


}
