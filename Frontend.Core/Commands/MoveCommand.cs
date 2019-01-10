using Caliburn.Micro;
using Frontend.Core.Navigation;
using Frontend.Core.ViewModels.Interfaces;

namespace Frontend.Core.Commands
{
	public class MoveCommand : CommandBase
	{
		private readonly IStepConductorBase conductorViewModel;

		public MoveCommand(IEventAggregator eventAggregator, IStepConductorBase conductorViewModel)
			 : base(eventAggregator)
		{
			this.conductorViewModel = conductorViewModel;
		}

		protected override bool OnCanExecute(object parameter)
		{
			if ((Direction)parameter == Direction.Backward)
			{
				return conductorViewModel.CanMoveBackward;
			}
			else if ((Direction)parameter == Direction.Forward)
			{
				return conductorViewModel.CanMoveForward;
			}
			else
			{
				return false;
			}
		}

		protected override void OnExecute(object parameter)
		{
			conductorViewModel.Move((Direction)parameter);
		}
	}
}