// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.IO;
using Microsoft.CodeAnalysis.Editor.CSharp.Interactive;
using Microsoft.CodeAnalysis.Editor.Interactive;
using Microsoft.VisualStudio.LanguageServices.Interactive;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.InteractiveWindow;
using Microsoft.VisualStudio.InteractiveWindow.Commands;
using Microsoft.VisualStudio.InteractiveWindow.Shell;
using LanguageServiceGuids = Microsoft.VisualStudio.LanguageServices.Guids;

namespace Microsoft.VisualStudio.LanguageServices.CSharp.Interactive
{
    [Export(typeof(CSharpVsInteractiveWindowProvider))]
    internal sealed class CSharpVsInteractiveWindowProvider : VsInteractiveWindowProvider
    {
        [ImportingConstructor]
        public CSharpVsInteractiveWindowProvider(
            SVsServiceProvider serviceProvider,
            IVsInteractiveWindowFactory interactiveWindowFactory,
            IViewClassifierAggregatorService classifierAggregator,
            IContentTypeRegistryService contentTypeRegistry,
            IInteractiveWindowCommandsFactory commandsFactory,
            [ImportMany]IInteractiveWindowCommand[] commands,
            VisualStudioWorkspace workspace)
            : base(serviceProvider, interactiveWindowFactory, classifierAggregator, contentTypeRegistry, commandsFactory, commands, workspace)
        {
        }

        protected override Guid LanguageServiceGuid
        {
            get { return LanguageServiceGuids.CSharpLanguageServiceId; }
        }

        protected override Guid Id
        {
            get { return CSharpVsInteractiveWindowPackage.Id; }
        }

        protected override string Title
        {
            // TODO: localize
            get { return "C# Interactive"; }
        }

        protected override InteractiveEvaluator CreateInteractiveEvaluator(
            SVsServiceProvider serviceProvider,
            IViewClassifierAggregatorService classifierAggregator,
            IContentTypeRegistryService contentTypeRegistry,
            VisualStudioWorkspace workspace)
        {
            return new CSharpInteractiveEvaluator(
                workspace.Services.HostServices,
                classifierAggregator,
                CommandsFactory,
                Commands,
                contentTypeRegistry,
                Path.GetDirectoryName(typeof(CSharpVsInteractiveWindowPackage).Assembly.Location),
                CommonVsUtils.GetWorkingDirectory());
        }
    }
}
