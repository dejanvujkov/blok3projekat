using System;

namespace FTN.Common
{
    public enum CurveStyle : short
    {

	  /// The Y-axis values are assumed constant until the next curve point and prior to the first curve point.
	  constantYValue,

	  /// An unspecified formula is assumed to compute the Y-axis value between points.
	  formula,

	  /// The Y-axis values are assumed to ramp between points.
	  rampYValue,

	  /// The Y-axis values are assumed to be a straight line between values.  Also known as linear interpolation.
	  straightLineYValues,
    }

    public enum UnitMultiplier : short
    {

	  /// Centi 10**-2.
	  c,

	  /// Deci 10**-1.
	  d,

	  /// Giga 10**9.
	  G,

	  /// Kilo 10**3.
	  k,

	  /// Milli 10**-3.
	  m,

	  /// Mega 10**6.
	  M,

	  /// Micro 10**-6.
	  micro,

	  /// Nano 10**-9.
	  n,

	  /// No multiplier or equivalently multiply by 1.
	  none,

	  /// Pico 10**-12.
	  p,

	  /// Tera 10**12.
	  T,
    }

    public enum UnitSymbol : short
    {

	  /// Current in ampere.
	  A,

	  /// Plane angle in degrees.
	  deg,

	  /// Relative temperature in degrees Celsius. In the SI unit system the symbol is ºC. Electric charge is measured in coulomb that has the unit symbol C. To destinguish degree Celsius form coulomb the symbol used in the UML is degC. Reason for not using ºC is the special character º is difficult to manage in software.
	  degC,

	  /// Capacitance in farad.
	  F,

	  /// Mass in gram.
	  g,

	  /// Time in hours.
	  h,

	  /// Inductance in henry.
	  H,

	  /// Frequency in hertz.
	  Hz,

	  /// Energy in joule.
	  J,

	  /// Length in meter.
	  m,

	  /// Area in square meters.
	  m2,

	  /// Volume in cubic meters.
	  m3,

	  /// Time in minutes.
	  min,

	  /// Force in newton.
	  N,

	  /// Dimension less quantity, e.g. count, per unit, etc.
	  none,

	  /// Resistance in ohm.
	  ohm,

	  /// Pressure in pascal (n/m2).
	  Pa,

	  /// Plane angle in radians.
	  rad,

	  /// Time in seconds.
	  s,

	  /// Conductance in siemens.
	  S,

	  /// Voltage in volt.
	  V,

	  /// Apparent power in volt ampere.
	  VA,

	  /// Apparent energy in volt ampere hours.
	  VAh,

	  /// Reactive power in volt ampere reactive.
	  VAr,

	  /// Reactive energy in volt ampere reactive hours.
	  VArh,

	  /// Active power in watt.
	  W,

	  /// Real energy in what hours.
	  Wh,
    }
}
