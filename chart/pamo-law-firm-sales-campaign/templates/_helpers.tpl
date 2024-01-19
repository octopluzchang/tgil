{{/*
Expand the name of the chart.
*/}}
{{- define "pamo-law-firm-sales-campaign.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" }}
{{- end }}
{{- define "pamoIndividualBatch.name" -}}
{{- default (printf "%s-batch" .Chart.Name) .Values.pamoIndividualBatch.nameOverride | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Create a default fully qualified app name.
We truncate at 63 chars because some Kubernetes name fields are limited to this (by the DNS naming spec).
If release name contains chart name it will be used as a full name.
*/}}
{{- define "pamo-law-firm-sales-campaign.fullname" -}}
{{- if .Values.fullnameOverride }}
{{- .Values.fullnameOverride | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- $name := default .Chart.Name .Values.nameOverride }}
{{- if contains $name .Release.Name }}
{{- .Release.Name | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- printf "%s-%s" .Release.Name $name | trunc 63 | trimSuffix "-" }}
{{- end }}
{{- end }}
{{- end }}

{{- define "pamoIndividualBatch.fullname" -}}
{{- if .Values.pamoIndividualBatch.fullnameOverride }}
{{- .Values.pamoIndividualBatch.fullnameOverride | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- $name := default (printf "%s-batch" .Chart.Name) .Values.pamoIndividualBatch.nameOverride }}
{{- printf "%s" $name | trunc 63 | trimSuffix "-" }}
{{- end }}
{{- end }}

{{/*
Create chart name and version as used by the chart label.
*/}}
{{- define "pamo-law-firm-sales-campaign.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Common labels
*/}}
{{- define "pamo-law-firm-sales-campaign.labels" -}}
helm.sh/chart: {{ include "pamo-law-firm-sales-campaign.chart" . }}
{{ include "pamo-law-firm-sales-campaign.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}

{{- define "pamoIndividualBatch.labels" -}}
helm.sh/chart: {{ include "pamo-law-firm-sales-campaign.chart" . }}
{{ include "pamoIndividualBatch.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}

{{/*
Selector labels
*/}}
{{- define "pamo-law-firm-sales-campaign.selectorLabels" -}}
app.kubernetes.io/name: {{ include "pamo-law-firm-sales-campaign.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}

{{- define "pamoIndividualBatch.selectorLabels" -}}
app.kubernetes.io/name: {{ include "pamoIndividualBatch.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}

{{/*
Create the name of the service account to use
*/}}
{{- define "pamo-law-firm-sales-campaign.serviceAccountName" -}}
{{- if .Values.serviceAccount.create }}
{{- default (include "pamo-law-firm-sales-campaign.fullname" .) .Values.serviceAccount.name }}
{{- else }}
{{- default "default" .Values.serviceAccount.name }}
{{- end }}
{{- end }}
