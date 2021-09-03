using NodeCanvas.Framework;
using ParadoxNotion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ability
{
	public abstract class AbilityNode : Node
	{
		public override int maxInConnections => 1;

		public override int maxOutConnections => 0;

		public override Type outConnectionType => typeof( AbilityConnection );

		public override bool allowAsPrime => true;

		public override bool canSelfConnect => false;

		public override Alignment2x2 commentsAlignment => Alignment2x2.Bottom;

		public override Alignment2x2 iconAlignment => Alignment2x2.Bottom;
	}
}

