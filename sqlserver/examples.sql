--select * from [dbo].[Users]


select COUNT(Id) as 'Number of users', UpVotes from [dbo].[Users]
where UpVotes > 5500
GROUP BY UpVotes
HAVING COUNT(Id) > 5
ORDER BY UpVotes


-- quero saber quantos usuários tem UpVotes maior que 500.