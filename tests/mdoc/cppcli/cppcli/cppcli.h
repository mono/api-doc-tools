// cppcli.h

#pragma once

using namespace System;

namespace cppcli {

	public class SomeClass {};

	public interface class SomeInterface  {
	public:
		SomeClass& SomeFunc2(SomeClass param) = 0;
		int SomeFunc(volatile SomeClass* param) = 0;
		SomeClass** SomeFunc3(int param) = 0;
		int SomeFunc4(SomeClass*& param, volatile int param2) = 0;
	};
}
