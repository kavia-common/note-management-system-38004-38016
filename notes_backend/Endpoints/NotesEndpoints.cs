using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NotesBackend.DTOs;
using NotesBackend.Models;
using NotesBackend.Repositories;

namespace NotesBackend.Endpoints
{
    /// <summary>
    /// Minimal API endpoint group for Notes CRUD operations.
    /// </summary>
    public static class NotesEndpoints
    {
        // PUBLIC_INTERFACE
        /// <summary>
        /// Registers the Notes endpoints under /api/notes.
        /// </summary>
        /// <param name="app">The web application.</param>
        public static void MapNotesEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/notes").WithTags("Notes");

            group.MapPost("/", CreateNote)
                .WithName("CreateNote")
                .WithSummary("Create a new note")
                .WithDescription("Creates a new note with a required title (max 200 chars) and optional content.")
                .Produces<NoteResponse>(StatusCodes.Status201Created)
                .ProducesValidationProblem(StatusCodes.Status400BadRequest);

            group.MapGet("/", GetNotes)
                .WithName("ListNotes")
                .WithSummary("List all notes")
                .WithDescription("Retrieves all notes, ordered by creation time descending.")
                .Produces<IEnumerable<NoteResponse>>(StatusCodes.Status200OK);

            group.MapGet("/{id:guid}", GetNoteById)
                .WithName("GetNoteById")
                .WithSummary("Get a note by Id")
                .WithDescription("Retrieves a single note by its unique identifier.")
                .Produces<NoteResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            group.MapPut("/{id:guid}", UpdateNote)
                .WithName("UpdateNote")
                .WithSummary("Update a note by Id")
                .WithDescription("Updates a note's title and content. Title is required with max 200 chars.")
                .Produces<NoteResponse>(StatusCodes.Status200OK)
                .ProducesValidationProblem(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound);

            group.MapDelete("/{id:guid}", DeleteNote)
                .WithName("DeleteNote")
                .WithSummary("Delete a note by Id")
                .WithDescription("Deletes an existing note by its unique identifier.")
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound);
        }

        private static Results<CreatedAtRoute<NoteResponse>, ValidationProblem> CreateNote(
            [FromBody] CreateNoteRequest request,
            INoteRepository repository)
        {
            var validation = ValidateCreateUpdate(request);
            if (validation is not null)
            {
                return TypedResults.ValidationProblem(validation);
            }

            var utcNow = DateTime.UtcNow;
            var note = new Note
            {
                Id = Guid.NewGuid(),
                Title = request.Title.Trim(),
                Content = request.Content ?? string.Empty,
                CreatedAt = utcNow,
                UpdatedAt = utcNow
            };

            repository.Create(note);

            var resp = ToResponse(note);
            return TypedResults.CreatedAtRoute(resp, routeName: "GetNoteById", routeValues: new { id = note.Id });
        }

        private static Results<Ok<IEnumerable<NoteResponse>>, EmptyHttpResult> GetNotes(INoteRepository repository)
        {
            var items = repository.GetAll().Select(ToResponse);
            return TypedResults.Ok(items);
        }

        private static Results<Ok<NoteResponse>, NotFound> GetNoteById(Guid id, INoteRepository repository)
        {
            var note = repository.GetById(id);
            if (note is null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(ToResponse(note));
        }

        private static Results<Ok<NoteResponse>, ValidationProblem, NotFound> UpdateNote(
            Guid id,
            [FromBody] UpdateNoteRequest request,
            INoteRepository repository)
        {
            var validation = ValidateCreateUpdate(request);
            if (validation is not null)
            {
                return TypedResults.ValidationProblem(validation);
            }

            var existing = repository.GetById(id);
            if (existing is null)
            {
                return TypedResults.NotFound();
            }

            existing.Title = request.Title.Trim();
            existing.Content = request.Content ?? string.Empty;
            existing.UpdatedAt = DateTime.UtcNow;

            repository.Update(existing);

            return TypedResults.Ok(ToResponse(existing));
        }

        private static Results<NoContent, NotFound> DeleteNote(Guid id, INoteRepository repository)
        {
            var ok = repository.Delete(id);
            if (!ok)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.NoContent();
        }

        private static Dictionary<string, string[]>? ValidateCreateUpdate(object req)
        {
            string? title = null;
            string? content = null;

            switch (req)
            {
                case CreateNoteRequest c:
                    title = c.Title;
                    content = c.Content;
                    break;
                case UpdateNoteRequest u:
                    title = u.Title;
                    content = u.Content;
                    break;
            }

            var errors = new Dictionary<string, string[]>();

            // Title validation
            if (string.IsNullOrWhiteSpace(title))
            {
                errors["Title"] = new[] { "Title is required." };
            }
            else if (title!.Trim().Length > 200)
            {
                errors["Title"] = new[] { "Title must be at most 200 characters." };
            }

            // Content can be empty/null; normalize later
            // If there are errors, return them for ValidationProblem
            return errors.Count > 0 ? errors : null;
        }

        private static NoteResponse ToResponse(Note n) => new NoteResponse
        {
            Id = n.Id,
            Title = n.Title,
            Content = n.Content,
            CreatedAt = n.CreatedAt,
            UpdatedAt = n.UpdatedAt
        };
    }
}
