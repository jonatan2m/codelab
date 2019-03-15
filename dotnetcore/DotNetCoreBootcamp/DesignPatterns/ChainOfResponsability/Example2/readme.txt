This example was based on https://www.developer.com/java/article.php/3745236/Working-With-Design-Patterns-Chain-of-Responsibility.htm
The problem:

Workflow-based systems are another area where the chain of responsibility pattern is particularly applicable.
Expense reimbursement is one common example of such a workflow system.

An employee typically submits an expense report to his or her manager, 
looking for approval and subsequent reimbursement for business travel expenses.
The manager (if in his or her office) often can approve the report immediately,
as long as it doesn't exceed a certain amount and no other special circumstances exist. 
If the manager cannot approve the expense report, it moves along the chain to the next appropriate person. 
The next person might be a VP, or a peer manager if the original manager happens to be out of the office. 
Different rules and powers apply to the VP, and every once in a while, a big shot will have to get involved. 
In all cases, the person currently holding the expense report knows who the next person in line is.

Listing 1 shows the relevant code for an ExpenseReport class.
This class tracks the amount, whether or not the expense involve international travel (a special case),
and the person who ultimately handled (approved or rejected) the report.

Listing 2 presents the Approver class, the core of the chain of responsibility pattern.
The Approver class represents the entity known in the pattern as the Handler. 
It's typically an abstract class.
In this case, it could be represented as either;
I've chosen to implement three specific handler subtypes: Manager, VicePresident, and CEO (see Listing 3).
An AutoRejectHandler also exists; approvers of this type reject everything.

The client sends an ExpenseReport object to an Approver using the handle method.
Code in the handle method determines whether to send the report on or to approve it.
The report is sent on if the approver isn't allowed to handle it (too much money, 
or it represents international travel and they aren't cleared for that) or if 
the approver is out of the office.

Each of the Approver subclasses constrains the approval details.
- Managers can approve up to $5,000, and only some managers can approve international travel. 
- Vice Presidents (VPs) can approve up to $100,000 and all international travel. 
- CEOs can approve everything, unless they're out of the office,
in which case the expense report automatically goes to an AutoRejectHandler (too bad for the employee!).