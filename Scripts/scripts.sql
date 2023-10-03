-- procedure for filtering books by genre or  name

create procedure dbo.GetFilteredBlogs(@genre nvarchar(max), @author nvarchar(max))
as 
begin
select b.* from dbo.Books b
inner join dbo.Genres g
on b.BookGenreGenreId = g.GenreId
inner join dbo.AuthorBooks ab on b.BookId = ab.BookId
inner join dbo.Authors a on ab.AuthorId = a.AuthorId
Where (@genre is null OR g.GenreName=@genre ) AND
(@author is null OR a.AuthorName=@author)
end


exec dbo.GetFilteredBlogs  @genre='Fiction',@author=null


-- Test Data for tables

insert into dbo.Genres  (GenreName) values ( 'Political'), ('Economics') , ('Literature'),
('Philosophy'), ('Fiction')
insert into dbo.Authors (AuthorName,DOB) values ('JKRollins', '1965-07-31'),
('Yuval Noah Harari', '1976-02-24')
insert into dbo.Books (Title, PublishedDate, BookGenreGenreId) 
values('HarryPotter',GETDATE(), 9), ('Sapiens', GETDATE(), 8)

