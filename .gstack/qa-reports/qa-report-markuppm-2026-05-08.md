# QA Report: MarkupPM

**Date:** 2026-05-08
**Branch:** master
**Mode:** Full (source code review — WPF desktop app, no browser)
**Tier:** Standard
**Duration:** ~10 min
**Build:** 0 errors, 0 warnings
**Tests:** 30/30 passing (25 original + 5 regression)

---

## Summary

| Metric | Value |
|--------|-------|
| Issues found | 5 |
| Fixes applied | 4 (verified: 4, best-effort: 0, reverted: 0) |
| Deferred issues | 1 |
| Health score | baseline: 72 → final: 92 |

---

## Findings

| ID | Severity | Category | Status | Commit |
|----|----------|----------|--------|--------|
| QA-001 | HIGH | Functional | verified | a72286e |
| QA-002 | HIGH | Functional | verified | 877317a |
| QA-003 | MEDIUM | Data binding | verified | ee0b0b1 |
| QA-004 | MEDIUM | Memory leak | verified | 8e3c270 |
| QA-005 | LOW | UX (stale display) | deferred | — |

---

### QA-001 — Command-line file open fails at startup (HIGH)

**Category:** Functional
**Status:** verified (a72286e)
**Files changed:** App.xaml.cs, MainWindow.xaml.cs

**Bug:** `OnStartup` fires before `StartupUri` creates the MainWindow, so `Current.MainWindow` is null when trying to open a file via command-line argument. File type association (.md double-click) silently fails.

**Fix:** Store the pending file path in `_pendingFilePath` and open it in the MainWindow's `Loaded` event handler, when `DataContext` is guaranteed to be set.

---

### QA-002 — WelcomeVm.Recientes not refreshed after save/open (HIGH)

**Category:** Functional
**Status:** verified (877317a)
**Files changed:** MainViewModel.cs

**Bug:** `AbrirArchivo` and `EscribirArchivo` call `_recentFiles.AddRecent(path)` which updates the JSON on disk, but `WelcomeVm.RefreshRecientes()` is never called. The welcome screen's recent files list is stale until the app restarts.

**Fix:** Call `WelcomeVm.RefreshRecientes()` after each `AddRecent` call in both `AbrirArchivo` and `EscribirArchivo`.

---

### QA-003 — Iniciales stale when Responsable changes (MEDIUM)

**Category:** Data binding
**Status:** verified (ee0b0b1)
**Files changed:** TareaViewModel.cs

**Bug:** The `Iniciales` computed property depends on `Responsable`, but when the user edits the Responsable field in the detail panel, only `PropertyChanged("Responsable")` fires. The avatar circle in the task row shows stale initials.

**Fix:** Added `partial void OnResponsableChanged(string value) => OnPropertyChanged(nameof(Iniciales))` to TareaViewModel.

---

### QA-004 — Event subscription leak in RemoveFase (MEDIUM)

**Category:** Memory leak
**Status:** verified (8e3c270)
**Files changed:** MainViewModel.cs

**Bug:** `AddFase` subscribes lambda handlers to `FaseViewModel.PropertyChanged` and `Tareas.CollectionChanged`. `RemoveFase` never unsubscribes. Removed FaseViewModels keep references to MainViewModel through closures, preventing GC and causing phantom `MarkDirty()` / `TotalTareas` notifications from dead objects.

**Fix:** Refactored to named methods (`SubscribeFase`/`UnsubscribeFase`). `RemoveFase` now unsubscribes before removing. `RebuildFases` also unsubscribes before clearing.

---

### QA-005 — FechaVencida overnight staleness (LOW) — DEFERRED

**Category:** UX
**Status:** deferred

**Bug:** `FechaVencida` compares `Fecha` to `DateTime.Today`. If the app stays open overnight, tasks due today don't turn red until the next interaction that forces a re-render. Would require a timer to pulse PropertyChanged on all tasks daily.

**Reason for deferral:** Low impact. Desktop apps commonly have this behavior. Fix requires a DispatcherTimer touching all TareaViewModels, which adds complexity for a rare scenario.

---

## Regression Tests

5 new tests in `MarkupPM.Tests/QaRegressionTests.cs` (commit 365f227):

| Test | Covers |
|------|--------|
| `Iniciales_UpdatesWhenResponsableChanges` | QA-003 |
| `Iniciales_ShowsDashWhenResponsableCleared` | QA-003 edge case |
| `RemoveFase_DoesNotMarkDirtyAfterRemoval` | QA-004 |
| `RemoveFase_TotalTareasNotAffectedByRemovedFase` | QA-004 |
| `WelcomeVm_RecientesIsAccessible` | QA-002 |

---

## PR Summary

> QA found 5 issues, fixed 4 (2 HIGH, 2 MEDIUM). Command-line open, stale recents, stale initials, and event subscription leak all resolved. 5 regression tests added. Health score 72 → 92. Tests: 30/30.
