﻿using System;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Continental.API.WebApi.Dependencies
{
    public static class FluentValidationDependencyInjection
    {
        public static IServiceCollection AgregarFluentValidation(this IMvcBuilder mvc, IServiceCollection services)
        {
            mvc.AddFluentValidation(config =>
            {
                config.RegisterValidatorsFromAssemblyContaining<Startup>();
                config.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
            });

            services.AddTransient<IValidatorInterceptor, ValidatorInterceptor>();

            return services;
        }
    }

    public class ValidatorInterceptor : IValidatorInterceptor
    {
        private readonly ILogger<ValidatorInterceptor> _logger;

        public ValidatorInterceptor(ILogger<ValidatorInterceptor> logger)
        {
            _logger = logger;
        }

        public IValidationContext BeforeMvcValidation(ControllerContext controllerContext, IValidationContext commonContext)
        {
            return commonContext;
        }

        public ValidationResult AfterMvcValidation(ControllerContext controllerContext, IValidationContext commonContext,
            ValidationResult result)
        {
            try
            {
                if (!result.IsValid)
                {
                    var model = commonContext.InstanceToValidate;
                    var controller = $"{controllerContext.ActionDescriptor.ControllerName}.{controllerContext.ActionDescriptor.ActionName}";
                    _logger.LogWarning("Modelo invalido en {controller} {@Request}", controller, model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrio un error en ValidatorInterceptor");
            }

            return result;
        }
    }
}
