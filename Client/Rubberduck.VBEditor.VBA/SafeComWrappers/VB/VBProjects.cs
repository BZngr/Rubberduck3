using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Rubberduck.Unmanaged;
using Rubberduck.Unmanaged.Abstract.SafeComWrappers;
using Rubberduck.Unmanaged.Abstract.SafeComWrappers.VB;
using Rubberduck.Unmanaged.Abstract.SafeComWrappers.VB.Enums;
using Rubberduck.Unmanaged.Events;
using VB = Microsoft.Vbe.Interop;

// ReSharper disable once CheckNamespace - Special dispensation due to conflicting file vs namespace priorities
namespace Rubberduck.VBEditor.SafeComWrappers.VBA
{
    public sealed class VBProjects : SafeEventedComWrapper<VB.VBProjects, VB._dispVBProjectsEvents>, IVBProjects, VB._dispVBProjectsEvents
    {
        public VBProjects(VB.VBProjects target, bool rewrapping = false)
        :base(target, rewrapping)
        {
        }

        public int Count => IsWrappingNullReference ? 0 : Target.Count;

        public IVBE VBE => new VBE((IsWrappingNullReference ? null : Target.VBE)!);

        public IVBE Parent => new VBE((IsWrappingNullReference ? null : Target.Parent)!);

        public IVBProject Add(ProjectType type)
        {
            return new VBProject((IsWrappingNullReference ? null : Target.Add((VB.vbext_ProjectType)type))!);
        }

        public void Remove(IVBProject project)
        {
            if (IsWrappingNullReference)
            {
                return;
            }
            Target.Remove((VB.VBProject)project.Target);
        }

        public IVBProject Open(string path)
        {
            return new VBProject((IsWrappingNullReference ? null : Target.Open(path))!);
        }

        // Not applicable to VBA
        IVBProject IVBProjects.StartProject
        {
            get => new VBProject(null!);
            set { }
        }

        public IVBProject this[object index] => new VBProject((IsWrappingNullReference ? null : Target.Item(index))!);

        IEnumerator<IVBProject> IEnumerable<IVBProject>.GetEnumerator()
        {
            return new ComWrapperEnumerator<IVBProject>(Target, comObject => new VBProject((VB.VBProject) comObject));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return IsWrappingNullReference
                ? new List<IEnumerable>().GetEnumerator()
                : ((IEnumerable<IVBProject>) this).GetEnumerator();
        }

        public override bool Equals(ISafeComWrapper<VB.VBProjects> other)
        {
            return IsEqualIfNull(other) || (other != null && ReferenceEquals(other.Target, Target));
        }

        public bool Equals(IVBProjects? other)
        {
            return Equals((other as SafeComWrapper<VB.VBProjects>)!);
        }

        public override int GetHashCode()
        {
            return IsWrappingNullReference ? 0
                : HashCode.Combine(Target);
        }

        protected override void Dispose(bool disposing) => base.Dispose(disposing);

        #region Events

        public event EventHandler<ProjectEventArgs> ProjectAdded = delegate { };
        void VB._dispVBProjectsEvents.ItemAdded([MarshalAs(UnmanagedType.Interface), In] VB.VBProject VBProject)
        {
            OnDispatch(ProjectAdded, VBProject, true);
        }

        public event EventHandler<ProjectEventArgs> ProjectRemoved = delegate { };
        void VB._dispVBProjectsEvents.ItemRemoved([MarshalAs(UnmanagedType.Interface), In] VB.VBProject VBProject)
        {
            OnDispatch(ProjectRemoved, VBProject);
        }

        public event EventHandler<ProjectRenamedEventArgs> ProjectRenamed = delegate { };
        void VB._dispVBProjectsEvents.ItemRenamed([MarshalAs(UnmanagedType.Interface), In] VB.VBProject VBProject,
            [MarshalAs(UnmanagedType.BStr), In] string OldName)
        {
            using (var project = new VBProject(VBProject))
            {
                if (!IsInDesignMode() || VBProject.Protection == VB.vbext_ProjectProtection.vbext_pp_locked)
                {
                    return;
                }

                var projectId = project.ProjectId;

                if (projectId == null)
                {
                    return;
                }

                var handler = ProjectRenamed;
                handler?.Invoke(this, new ProjectRenamedEventArgs(projectId, project.Name, OldName));
            }
        }

        public event EventHandler<ProjectEventArgs> ProjectActivated = delegate { };
        void VB._dispVBProjectsEvents.ItemActivated([MarshalAs(UnmanagedType.Interface), In] VB.VBProject VBProject)
        {
            OnDispatch(ProjectActivated, VBProject);
        }

        private void OnDispatch(EventHandler<ProjectEventArgs> dispatched, VB.VBProject vbProject, bool assignId = false)
        {
            using (var project = new VBProject(vbProject))
            {
                var handler = dispatched;
                if (handler == null || !IsInDesignMode() ||
                    vbProject.Protection == VB.vbext_ProjectProtection.vbext_pp_locked)
                {
                    return;
                }

                if (assignId)
                {
                    project.AssignProjectId();
                }

                var projectId = project.ProjectId;

                if (projectId == null)
                {
                    return;
                }

                handler.Invoke(project, new ProjectEventArgs(projectId, project.Name));
            }
        }

        private bool IsInDesignMode()
        {
            foreach (var project in this)
                using(project)
                {
                    if (project.Mode != EnvironmentMode.Design)
                    {
                        return false;
                    }
                }
            return true;
        }

        #endregion
    }
}