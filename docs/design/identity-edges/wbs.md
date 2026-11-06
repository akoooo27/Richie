# Identity edges workstream: WBS and progress

Started 2026-11-05. PM: Claude (main session). Canvas: Richie Identity Edges, https://claude.ai/artifact/6Xe7bnSdGHE3xvVR5LeMge (created 2026-11-05, empty until assembly).
Working files: `.design/edges/` (git-ignored). Docs: `docs/design/identity-edges/`.

Scope: edge pages (session expired, generic error, offline, too many requests, signed out); account settings gaps for passkeys and email; account recovery when email is lost; SSO placement in sign-in and settings.

| Phase | WP | Owner | Status |
|---|---|---|---|
| 1 | Suiss guide + catalog | earlier program | done (reused) |
| 1 | WP1.1 Suiss addendum (edge, recovery, SSO blocks) | Sonnet | done: suiss-addendum.md |
| 2 | WP2.1 how-we-design.md Revision 2 | Opus | done: ../how-we-design.md Rev 2 |
| 2 | WP2.2 Coverage inventory | Sonnet | done: inventory.md |
| 3 | WP3.0 Product decisions | PM + user | done: SSO = Google/Microsoft only; recovery = passkey first, then support (72 h hold); item 2 = fill gaps |
| 3 | WP3.1 Spec foundation (plan, decisions, IA, grammar, edges.css, reference boards, canvas.json) | Opus | done: spec.md Part A, 9 boards, rig 0/0; published as canvas v3 |
| 3 | WP3.2 Edge (17) · WP3.3 Settings gaps (15) · WP3.4 Recovery (18) · WP3.5 SSO sign-in (14) | Opus x4 | done: 64 boards + spec-edge/settings/recovery/sso.md; each 0 blockers / 0 majors on own rig |
| 3 | WP3.6 Canvas assembly + publish | PM | done: 7 shared CSS requests merged, canvas.json sizes/rows, links verified, spec Part B; canvas version 1793888623-5e41 (73 boards) |
| 4 | WP4.1 Rig sweep + layout shift (Sonnet) · WP4.2 Keyboard/SR WCAG 2.2 (Opus) · WP4.3 Contrast (Haiku) · WP4.4 Copy (Sonnet) · WP4.5 State coverage + buildability (Sonnet) | mixed | running in parallel |
| 4 | WP4.5-4.6 Fix + regression | Opus/Sonnet | waiting |
| 5 | WP5.1-5.3 Independent grade, revise, re-grade | Opus (fresh) | waiting |

## Log
- 2026-11-05: Phase 1 reused (guide + catalog) plus WP1.1 addendum. Phase 2 delivered (how-we-design Rev 2, inventory). Decisions from the user recorded. Canvas created; shiba PNG and SVG copied as assets; rig patched to keep blob extensions. WP3.1 started.
- 2026-11-05: WP3.1 done (73-board plan). PM decisions P-1..P-7 written into spec §9 (labels "Sign in with Google/Microsoft", "Sign up with ..." on Create account; Google G as PNG blob 5162155e…; 20-minute sign-in request; 72 h hold from approval; reply "within 2 business days"; Security base frozen for WP3.3). Foundation published (canvas version 1793886071-0534). WP3.2–3.5 launched.
- 2026-11-05: Phase 3 done. All four domain WPs delivered; PM merged CSS requests, applied canvas changes, ruled B-1..B-5 (spec Part B), published 73 boards. Rig gained BOARD_THEME=dark. Phase 4 launched with five testers.

## Paused 2026-11-05 (user hit usage limits) — resume here

State when paused:
- Phases 1–3 done; Phase 4 tests done (qa/p4-rig.md, p4-a11y.md, p4-contrast.md, p4-copy.md, p4-coverage.md); test log drafted (qa/test-log.md, §5 Retest pending).
- PM rulings B-1…B-33 in spec.md Part B. Shared CSS fixes in edges.css §9 (PM).
- Fix round: **WP3.3 done** (15/15, Security base changed), **WP3.5 done** (16/16, new board Provider-not-linked). **WP3.2 stopped** near the end (boards fixed; was writing the spec-edge fix log). **WP3.4 stopped** mid-way (was writing spec-recovery Revision 2 + fix log; had not yet re-copied Security-recovered from the changed Security base; two block labels on Recover-declined / Recover-limited must fit 256 px at 16 px).
- Canvas last published: version 1793888623-5e41 (73 boards, pre-fix). Local `.design/edges/project/` holds the fixed boards.

Next steps:
1. Resume WP3.2 and WP3.4 (SendMessage to their agents, or new agents briefed from this file + qa reports): finish fixes, fix logs, rig + text-spacing checks.
2. PM: add new boards to canvas.json from CANVAS-CHANGES (Signedout-client, Limited-dark, Signin-limited-dark, Security-load-error-dark, Recover-limited-dark, Verify-help, Provider-not-linked), link check, publish all.
3. WP4.6 regression (Sonnet) on changed boards + re-verify every finding; fill test-log §5. B-26 Part A sync (Sonnet).
4. Phase 5: fresh Opus grader (boards + specs + criteria only), evaluation artifact; revise anything < 8, re-grade.

## Resumed 2026-11-05 (after pause)
- Fix round finished: WP3.2 9/9, WP3.3 15/15, WP3.4 26/27 (COPY-31 won't fix), WP3.5 16/16. 80 boards published (canvas version 1793896016-b1a5).
- WP4.6 regression: 0 blockers / 0 majors light + dark; 95/95 findings have a status; REG-01 fixed by PM; REG-02 closed by the Part A sync (spec.md Part A Revision 2). Test log complete (qa/test-log.md). Phase 4 exit criteria hold.
- PM rulings now B-1…B-36.
- Phase 5: fresh Opus grader running (23 units + overall) → evaluation.md / evaluation.json. Next: publish evaluation artifact; revise any unit < 8 once; re-grade with a fresh agent.
- Phase 5 grade 1: overall 8.1 Ship with fixes; 19 Ship, 3 Ship with fixes (E9/E10 8.0, R-B 8.1, P-A 8.2), 1 No-ship (R-C 7.9: B-36 expired state undesigned). evaluation.md/json.
- PM: precedence rule unified (B-37) in all five spec headers; rulings B-37…B-43; `form + .ri-or` 12 px gap (edges.css §9).
- Revision round: one fresh Opus agent applying 14 fixes (R-C first, then must-fixes, then should-fixes). Next: canvas.json for its new boards (Recover-expired, Recover-review-cancel, Recover-owner-view, Signin-timeout-warning), publish, fresh re-grade, evaluation artifact.
- Revision round done (14/14). Re-grade: **8.6 overall, Ship; all 23 units Ship** (R-C 7.9 → 8.6). evaluation-round2.md/json. PM ruling B-44; post-grade polish (divider rule widened, 3 board heights). Canvas version 1793901647-b8c4, 84 boards. Review artifact: https://claude.ai/artifact/JYATTqX1AzJphazrHeTm6B.
- **Workstream complete.** Open follow-ups: Part A absorbs this round's rulings (6 places); busy/countdown row and two-exit error variant drawn; brand sign-off (release gate); handoffs to the identity and settings canvases. Nothing committed yet.
