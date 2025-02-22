﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Pagination.EntityFrameworkCore.Extensions
{
	public static partial class PaginationExtensions
	{
		//*** DbContext source
		public static async Task<Pagination<TSource>> AsPaginationAsync<TSource>(this DbContext source, int page, int limit, string sortColumn = "", bool orderByDescending = false) where TSource : class
		{
			PaginationExtensionsHelper.ValidateInputs(page, limit);

			var totalItems = await source.Set<TSource>().CountAsync().ConfigureAwait(false);

			if (!string.IsNullOrEmpty(sortColumn))
			{
				if (orderByDescending)
				{
					var resultsDesc = source.Set<TSource>().OrderByDescending(p => EF.Property<object>(p, sortColumn)).Skip((page - 1) * limit).Take(limit);
					return new Pagination<TSource>(await resultsDesc.ToListAsync().ConfigureAwait(false), totalItems, page, limit);
				}
				else
				{
					var resultsAsc = source.Set<TSource>().OrderBy(p => EF.Property<object>(p, sortColumn)).Skip((page - 1) * limit).Take(limit);
					return new Pagination<TSource>(await resultsAsc.ToListAsync().ConfigureAwait(false), totalItems, page, limit);
				}
			}
			var results = source.Set<TSource>().Skip((page - 1) * limit).Take(limit);

			return new Pagination<TSource>(await results.ToListAsync().ConfigureAwait(false), totalItems, page, limit);
		}

		public static async Task<Pagination<TSource>> AsPaginationAsync<TSource>(this DbContext dbContext, int page, int limit, Expression<Func<TSource, bool>> expression, string sortColumn = "", bool orderByDescending = false) where TSource : class
		{
			PaginationExtensionsHelper.ValidateInputs(page, limit);

			var totalItems = await dbContext.Set<TSource>().Where(expression).CountAsync().ConfigureAwait(false);
			var results = Enumerable.Empty<TSource>();
			if (!string.IsNullOrEmpty(sortColumn))
			{
				results = await (orderByDescending ? dbContext.Set<TSource>().Where(expression).OrderByDescending(p => EF.Property<object>(p, sortColumn)) : dbContext.Set<TSource>().Where(expression).OrderBy(p => EF.Property<object>(p, sortColumn))).Skip((page - 1) * limit).Take(limit).ToListAsync().ConfigureAwait(false);
			}
			else
			{
				results = await dbContext.Set<TSource>().Where(expression).Skip((page - 1) * limit).Take(limit).ToListAsync().ConfigureAwait(false);
			}
			return new Pagination<TSource>(results, totalItems, page, limit);
		}

		// PaginationAuto Mapping
		public static async Task<PaginationAuto<TSource, Tdestination>> AsPaginationAsync<TSource, Tdestination>(this DbContext dbContext, int page, int limit, Func<TSource, Tdestination> convertTsourceToTdestinationMethod, string sortColumn = "", bool orderByDescending = false) where TSource : class
		{
			PaginationExtensionsHelper.ValidateInputs(page, limit);

			var totalItems = await dbContext.Set<TSource>().CountAsync().ConfigureAwait(false);
			if (!string.IsNullOrEmpty(sortColumn))
			{
				if (orderByDescending)
				{
					var resultsDesc = dbContext.Set<TSource>().OrderByDescending(p => EF.Property<object>(p, sortColumn)).Skip((page - 1) * limit).Take(limit);
					return new PaginationAuto<TSource, Tdestination>(await resultsDesc.ToListAsync().ConfigureAwait(false), totalItems, convertTsourceToTdestinationMethod, page, limit);
				}
				else
				{
					var resultsAsc = dbContext.Set<TSource>().OrderBy(p => EF.Property<object>(p, sortColumn)).Skip((page - 1) * limit).Take(limit);
					return new PaginationAuto<TSource, Tdestination>(await resultsAsc.ToListAsync().ConfigureAwait(false), totalItems, convertTsourceToTdestinationMethod, page, limit);
				}
			}
			var results = dbContext.Set<TSource>().Skip((page - 1) * limit).Take(limit);

			return new PaginationAuto<TSource, Tdestination>(await results.ToListAsync().ConfigureAwait(false), totalItems, convertTsourceToTdestinationMethod, page, limit);
		}

		public static async Task<PaginationAuto<TSource, Tdestination>> AsPaginationAsync<TSource, Tdestination>(this DbContext dbContext, int page, int limit, Expression<Func<TSource, bool>> expression, Func<TSource, Tdestination> convertTsourceToTdestinationMethod, string sortColumn = "", bool orderByDescending = false) where TSource : class
		{
			PaginationExtensionsHelper.ValidateInputs(page, limit);

			var totalItems = await dbContext.Set<TSource>().Where(expression).CountAsync().ConfigureAwait(false);
			var results = Enumerable.Empty<TSource>();
			if (!string.IsNullOrEmpty(sortColumn))
			{
				results = await (orderByDescending ? dbContext.Set<TSource>().Where(expression).OrderByDescending(p => EF.Property<object>(p, sortColumn)) : dbContext.Set<TSource>().Where(expression).OrderBy(p => EF.Property<object>(p, sortColumn))).Skip((page - 1) * limit).Take(limit).ToListAsync().ConfigureAwait(false);
			}
			else
			{
				results = await dbContext.Set<TSource>().Where(expression).Skip((page - 1) * limit).Take(limit).ToListAsync().ConfigureAwait(false);
			}
			return new PaginationAuto<TSource, Tdestination>(results, totalItems, convertTsourceToTdestinationMethod, page, limit);
		}

		// PaginationAuto Async Mapping
		public static async Task<PaginationAuto<TSource, Tdestination>> AsPaginationAsync<TSource, Tdestination>(this DbContext dbContext, int page, int limit, Task<Func<TSource, Tdestination>> convertTsourceToTdestinationMethod, string sortColumn = "", bool orderByDescending = false) where TSource : class
		{
			PaginationExtensionsHelper.ValidateInputs(page, limit);

			var totalItems = await dbContext.Set<TSource>().CountAsync().ConfigureAwait(false);
			if (!string.IsNullOrEmpty(sortColumn))
			{
				if (orderByDescending)
				{
					var resultsDesc = dbContext.Set<TSource>().OrderByDescending(p => EF.Property<object>(p, sortColumn)).Skip((page - 1) * limit).Take(limit);
					return new PaginationAuto<TSource, Tdestination>(await resultsDesc.ToListAsync().ConfigureAwait(false), totalItems, await convertTsourceToTdestinationMethod, page, limit);
				}
				else
				{
					var resultsAsc = dbContext.Set<TSource>().OrderBy(p => EF.Property<object>(p, sortColumn)).Skip((page - 1) * limit).Take(limit);
					return new PaginationAuto<TSource, Tdestination>(await resultsAsc.ToListAsync().ConfigureAwait(false), totalItems, await convertTsourceToTdestinationMethod, page, limit);
				}
			}
			var results = dbContext.Set<TSource>().Skip((page - 1) * limit).Take(limit);

			return new PaginationAuto<TSource, Tdestination>(await results.ToListAsync().ConfigureAwait(false), totalItems, await convertTsourceToTdestinationMethod, page, limit);
		}

		public static async Task<PaginationAuto<TSource, Tdestination>> AsPaginationAsync<TSource, Tdestination>(this DbContext dbContext, int page, int limit, Expression<Func<TSource, bool>> expression, Task<Func<TSource, Tdestination>> convertTsourceToTdestinationMethod, string sortColumn = "", bool orderByDescending = false) where TSource : class
		{
			PaginationExtensionsHelper.ValidateInputs(page, limit);

			var totalItems = await dbContext.Set<TSource>().Where(expression).CountAsync().ConfigureAwait(false);
			var results = Enumerable.Empty<TSource>();
			if (!string.IsNullOrEmpty(sortColumn))
			{
				results = await (orderByDescending ? dbContext.Set<TSource>().Where(expression).OrderByDescending(p => EF.Property<object>(p, sortColumn)) : dbContext.Set<TSource>().Where(expression).OrderBy(p => EF.Property<object>(p, sortColumn))).Skip((page - 1) * limit).Take(limit).ToListAsync().ConfigureAwait(false);
			}
			else
			{
				results = await dbContext.Set<TSource>().Where(expression).Skip((page - 1) * limit).Take(limit).ToListAsync().ConfigureAwait(false);
			}
			return new PaginationAuto<TSource, Tdestination>(results, totalItems, await convertTsourceToTdestinationMethod, page, limit);
		}
	}
}