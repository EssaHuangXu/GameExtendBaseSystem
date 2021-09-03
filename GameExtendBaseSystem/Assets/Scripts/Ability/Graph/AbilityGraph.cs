using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.Framework;
using System;

namespace Ability
{
	public class AbilityGraph : Graph
	{
		public override Type baseNodeType => typeof( AbilityNode );

		public override bool requiresAgent => false;

		public override bool requiresPrimeNode => true;

		public override bool isTree => true;

		public override bool allowBlackboardOverrides => true;

		public override bool canAcceptVariableDrops => true;
	}
}
