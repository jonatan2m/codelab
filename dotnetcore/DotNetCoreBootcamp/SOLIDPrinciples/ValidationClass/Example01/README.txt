Introduction

1. Don't implement validations in other objects but give the other objects a list
	of validations (using Dependency inversion).

2. When the validation uses a logical implication 
	(if condition P is true then requirement Q needs to be true),
	do not implement the condition and the requirement in 1 property,
	but create a 2 properties (condition and requirement).


These validations needs to be created:

1. A person is only allowed to consume alcohol when his or her age is 18 or higher.
2. The age of a person must be 0 of higher.

In the WorstSolution folder you can see a poor example of validation and the reasons to that.

An alternative solution would be: 1 class implements 1 validation. Advantages:

Object orientation possible
	Inheritance: 2 different valdiations can use the logic of the same base class.
	Reuse: For example: validating the age of a customer and a employee can be done
		with the same validation class: "Age must be 0 or higher".
	Polyformism: A collection of validations can be handled easily.

Methods, constants, variables, etc. only used by the validation can be placed (private) in a class. 
	If a validation is no longer needed, 
	just delete the class and all related code is deleted automatically.

Unit testing is much easier.

When new validations are added to validationList,
the AlcoholSeller class does not need any change (Open/close principle, Dependency inversion).