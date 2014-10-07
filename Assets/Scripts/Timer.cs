using UnityEngine;
using System.Collections;

public class Timer 
{
	private float _deltaTime;		// in seconds
	private float _intervalDelay;	// in seconds

	private int _currentInterval;
	private int _totalIntervals;

	private bool _running;
	private bool _ticked;
	private bool _complete;

	public Timer( float intervalDelay, int totalIntervals = 0 ) 
	{
		_intervalDelay = intervalDelay;
		_totalIntervals = totalIntervals;

		_deltaTime = 0.0f;
		_currentInterval = 0;

		_running = false;
		_ticked = false;
		_complete = false;
	}

	public void Update() 
	{
		if ( !_running ) 
		{
			return;
		}

		_ticked = false;
		_deltaTime += Time.deltaTime;

		if ( _deltaTime >= _intervalDelay ) 
		{
			_currentInterval++;
			_deltaTime = 0.0f;

			_ticked = true;

			if ( _currentInterval >= _totalIntervals && _totalIntervals > 0 ) 
			{
				_complete = true;
				Stop();
			}
		}
	}

	public void Start() 
	{
		if ( _running ) 
		{
			return;
		}

		if ( _currentInterval >= _totalIntervals && _totalIntervals > 0 ) 
		{
			return;
		}

		_running = true;
	}

	public void Stop() 
	{
		if ( !_running ) 
		{
			return;
		}

		_running = false;
	}

	public void Reset( bool startAfterReset = false ) 
	{
		_deltaTime = 0.0f;
		_currentInterval = 0;
		_running = false;
		_ticked = false;
		_complete = false;

		if ( startAfterReset ) 
		{
			Start();
		}
	}

	public bool IsRunning() 
	{
		return _running;
	}

	public bool IsTicked() 
	{
		return _ticked;
	}

	public bool IsComplete() 
	{
		return _complete;
	}
}