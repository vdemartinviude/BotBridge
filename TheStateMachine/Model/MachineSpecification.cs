using StatesAndEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheStateMachine.Model;

public class MachineSpecification
{
    public List<BaseState> States { get; set; } 
    public List<IGuard<BaseState, BaseState>> IntermediaryGuards { get; set; }
    public List<IGuard<BaseState>> FinalGuards { get; set; }
}
